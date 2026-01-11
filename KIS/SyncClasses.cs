using BepInEx;
namespace KIS;

internal partial class SyncManager
{
    public class SyncBaseInfo
    {
        public string hdPath = null;
        public object hdValue = null;
        public string kdPath = null;
        public object kdValue = null;
        public bool always_equal = false;

        public SyncBaseInfo(string hdPath, object hdValue, string kdPath, object kdValue, bool always_equal)
        {
            this.hdPath = hdPath;
            this.hdValue = hdValue;
            this.kdPath = kdPath;
            this.kdValue = kdValue;
            this.always_equal = always_equal;
        }
    }
    public abstract class SyncEntry(SyncManager.SyncBaseInfo info)
    {
        protected static Knight.PlayerData kd => Knight.PlayerData.instance;
        public abstract string KDPath { get; }
        public abstract string SubscribeHDPath { get; }
        public Action<object> dirty_action = null;
        public abstract SyncBaseInfo ToBaseInfo();
        protected virtual object GetKDValue()
        {
            if (kd.GetType().GetField(KDPath) != null)
            {
                return kd.GetType().GetField(KDPath).GetValue(kd);
            }
            else
            {
                throw new Exception("Knight.PlayerData don't have field " + KDPath);
            }
        }
        protected virtual void _SetKDValue(object value)
        {
            if (kd.GetType().GetField(KDPath) != null)
            {
                kd.GetType().GetField(KDPath).SetValue(kd, Convert.ChangeType(value, kd.GetType().GetField(KDPath).FieldType));
            }
            else
            {
                throw new Exception("Knight.PlayerData don't have field " + KDPath);
            }
        }
        public abstract bool ShouldApply();

        //return if dirty
        public abstract bool ApplyValue();

        public void H2KSyncData()
        {
            try
            {
                if (ShouldApply())
                {
                    if (ApplyValue())
                    {
                        dirty_action?.Invoke(GetKDValue());
                    }
                }
            }
            catch (Exception e)
            {
                ("H2KSyncData error " + e).LogError();
            }
        }


    }
    public class DefaultKDValue : SyncEntry
    {
        private string kdPath;
        private object kdValue;

        public DefaultKDValue(SyncBaseInfo info) : base(info)
        {
            this.kdPath = info.kdPath;
            this.kdValue = info.kdValue;
        }

        public override string KDPath => kdPath;

        public override string SubscribeHDPath => null;

        public override bool ApplyValue()
        {
            if (!Equals(GetKDValue(), kdValue))
            {
                _SetKDValue(kdValue);
                return true;
            }
            return false;
        }

        public override bool ShouldApply()
        {
            return true;
        }

        public override SyncBaseInfo ToBaseInfo()
        {
            return new(null, null, kdPath, kdValue, false);
        }
    }
    public class HD2KDSync : SyncEntry
    {
        protected string hdPath;
        protected object hdValue;
        protected string kdPath;
        protected object kdValue;
        protected bool always_equal;
        protected static PlayerData hd => PlayerData.instance;
        public HD2KDSync(SyncBaseInfo info) : base(info)
        {
            hdPath = info.hdPath;
            hdValue = info.hdValue;
            kdPath = info.kdPath;
            kdValue = info.kdValue;
            always_equal = info.always_equal;
        }

        public override string KDPath => kdPath;

        public override string SubscribeHDPath => hdPath;
        protected virtual object GetHDValue()
        {
            if (hd.GetType().GetField(hdPath) != null)
            {
                return hd.GetType().GetField(hdPath).GetValue(hd);
            }
            else if (hd.GetType().GetProperty(hdPath) != null)
            {
                return hd.GetType().GetProperty(hdPath).GetValue(hd);
            }
            else
            {
                throw new Exception("PlayerData has no field or Property " + hdPath);
            }
        }


        public override bool ApplyValue()
        {
            object res = always_equal ? GetHDValue() : kdValue;
            if (!Equals(res, GetKDValue()))
            {
                _SetKDValue(res);
                return true;
            }
            return false;
        }

        public override bool ShouldApply()
        {
            return always_equal || Equals(hdValue, GetHDValue());
        }

        public override SyncBaseInfo ToBaseInfo()
        {
            return new(hdPath, hdValue, kdPath, kdValue, always_equal);
        }
    }

    public class FromUnlockedSlots : HD2KDSync
    {
        private object unlockedSlots;
        public const string CheckKey = "ToolEquips";
        public FromUnlockedSlots(SyncBaseInfo info) : base(info)
        {
            unlockedSlots = info.hdValue;
        }

        public override string SubscribeHDPath => CheckKey;

        protected override object GetHDValue()
        {
            int cnt = 0;
            foreach (var crest in hd.ToolEquips.GetValidDatas())
            {
                if (!crest.IsUnlocked || crest.Slots == null)
                {
                    continue;
                }
                cnt += crest.Slots.Count((slot) => slot.IsUnlocked);
            }
            return cnt;
        }
    }
    public class FromTool : HD2KDSync
    {
        public const string prefix = "Tool_";
        private string tool_name;

        public FromTool(SyncBaseInfo info) : base(info)
        {
            tool_name = hdPath.Replace(prefix, "");
        }

        public override string SubscribeHDPath => tool_name;

        protected override object GetHDValue()
        {
            if (!hd.Tools.GetValidNames().Contains(tool_name))
            {
                return false;
            }
            var tool_data = hd.Tools.GetData(tool_name);

            return tool_data.IsUnlocked && !tool_data.IsHidden;
        }

    }

    static Dictionary<string, Action<object>> dirty_action_dict = new()
    {
        {nameof(kd.nailDamage),(val)=>PlayMakerFSM.BroadcastEvent("UPDATE NAIL DAMAGE")},
        {nameof(kd.charmSlots),(val)=>{kd.charmSlots=Math.Max(Math.Min(kd.charmSlots,11),3);}},
        {nameof(kd.maxHealthBase),(val)=>{kd.maxHealthBase=Math.Min(kd.maxHealthBase,9);}},
        {nameof(kd.nailSmithUpgrades),(val)=>{SyncManager.Instance.H2KSyncData(nameof(hd.nailDamage));}}
    };
    private void AddNecessarySyncAction(SyncEntry entry)
    {
        entry.dirty_action += (val) =>
        {
            (entry.KDPath + " is dirty, set value to " + val).LogInfo();
        };

        if (dirty_action_dict.ContainsKey(entry.KDPath))
        {
            entry.dirty_action += dirty_action_dict[entry.KDPath];
        }

    }
    public SyncEntry FromBaseInfo(SyncBaseInfo info)
    {
        SyncEntry res;
        if (info.hdPath.IsNullOrWhiteSpace())
        {
            res = new DefaultKDValue(info);
        }
        else if (info.hdPath == FromUnlockedSlots.CheckKey)
        {
            res = new FromUnlockedSlots(info);
        }
        else if (info.hdPath.StartsWith(FromTool.prefix))
        {
            res = new FromTool(info);
        }
        else
        {
            res = new HD2KDSync(info);
        }
        AddNecessarySyncAction(res);
        return res;
    }

}