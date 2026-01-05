using System.IO;
using BepInEx;
using GenericVariableExtension;
using Newtonsoft.Json;
namespace KIS;

internal partial class SyncManager
{
    private static SyncManager _instance;
    public static SyncManager Instance => _instance ??= new SyncManager();
    private static PlayerData hd => PlayerData.instance;
    private static Knight.PlayerData kd => Knight.PlayerData.instance;

    private List<SyncEntry> sync_entries = new();
    private List<SyncBaseInfo> base_infos = new();

    private Dictionary<string, List<SyncEntry>> hdpath_to_sync_entry = new();

    private bool enable = true;
    private bool initialized = false;


    public SyncManager()
    {
    }

    private void ResetConfig()
    {
        sync_entries.Clear();
        hdpath_to_sync_entry.Clear();
        DefaultConfig();
    }
    private SyncBaseInfo SameNameSync(string path)
    {
        // they should have the same name and value
        return new SyncBaseInfo(path, null, path, null, true);
    }
    private SyncBaseInfo SameValueSync(string hdPath, string kdPath)
    {
        // they should have the same value
        return new SyncBaseInfo(hdPath, null, kdPath, null, true);
    }
    private SyncBaseInfo DefaultValue(string kdPath, object kdValue)
    {
        return new(null, null, kdPath, kdValue, false);
    }
    private SyncBaseInfo Tool2Charm(Tool tool, Charm charm)
    {
        return new(FromTool.prefix + tool.GetToolName(), null, charm.GetCharmName(), null, true);
    }


    private void DefaultConfig()
    {
        base_infos = [
            //movement
            SameNameSync(nameof(hd.hasDash)),
            SameValueSync(nameof(hd.hasDash), nameof(kd.canDash)),
            SameNameSync(nameof(hd.hasWalljump)),
            SameNameSync(nameof(hd.hasDoubleJump)),
            SameValueSync(nameof(hd.hasBrolly), nameof(kd.hasSuperDash)),
            // spells

            DefaultValue(nameof(kd.fireballLevel), 0),
            new (nameof(hd.hasNeedleThrow), true, nameof(kd.fireballLevel), 1, false),
            new (nameof(hd.hasSilkCharge), true, nameof(kd.fireballLevel), 2, false),
            DefaultValue(nameof(kd.quakeLevel), 0),
            new (nameof(hd.hasParry), true, nameof(kd.quakeLevel), 1, false),
            new (nameof(hd.hasSilkBossNeedle), true, nameof(kd.quakeLevel), 2, false),
            DefaultValue(nameof(kd.screamLevel), 0),
            new (nameof(hd.hasThreadSphere), true, nameof(kd.screamLevel), 1, false),
            new (nameof(hd.hasSilkBomb), true, nameof(kd.screamLevel), 2, false),
            //upgrades
            SameNameSync(nameof(hd.maxHealthBase)),
            SameNameSync(nameof(hd.nailDamage)),
            SameValueSync(nameof(hd.nailUpgrades), nameof(kd.nailSmithUpgrades)),

            //misc
            SameValueSync(nameof(hd.hasNeedolin), nameof(kd.hasDreamNail)),
            SameValueSync(nameof(hd.UnlockedFastTravelTeleport), nameof(kd.hasDreamGate)),
            SameValueSync(nameof(hd.permadeathMode), nameof(kd.permadeathMode)),
            SameNameSync(nameof(hd.bossRushMode)),
            SameValueSync(nameof(hd.HasBoundCrestUpgrader), nameof(kd.salubraBlessing)),
            SameValueSync(nameof(hd.hasChargeSlash), nameof(kd.hasNailArt)),
            SameValueSync(nameof(hd.hasChargeSlash), nameof(kd.hasCyclone)),
            SameValueSync(nameof(hd.hasChargeSlash), nameof(kd.hasDashSlash)),
            SameValueSync(nameof(hd.hasChargeSlash), nameof(kd.hasUpwardSlash)),

            //charm
            new(FromUnlockedSlots.CheckKey,null,nameof(kd.charmSlots),null,true),
            Tool2Charm(Tool.Compass,Charm.WaywardCompass)


        ];
        ApplyInfo();

    }


    private void ApplyInfo()
    {
        sync_entries.Clear();
        hdpath_to_sync_entry.Clear();
        foreach (var info in base_infos)
        {
            var new_entry = FromBaseInfo(info);
            sync_entries.Add(new_entry);
            if (!new_entry.SubscribeHDPath.IsNullOrWhiteSpace())
            {
                if (!hdpath_to_sync_entry.ContainsKey(new_entry.SubscribeHDPath))
                {
                    hdpath_to_sync_entry.Add(new_entry.SubscribeHDPath, new());
                }
                hdpath_to_sync_entry[new_entry.SubscribeHDPath].Add(new_entry);
            }
        }

    }
    public void Initialize()
    {
        if (initialized) return;
        initialized = true;
        // Initialization logic here
        if (File.Exists(HelperFun.GetSyncConfigPath()))
        {
            try
            {
                LoadCustomizeSyncConfig(HelperFun.GetSyncConfigPath());
            }
            catch (Exception e)
            {
                "Load CustomSyncConfig Failed, Use Default Config".LogWarning();
                e.LogWarning();
                ResetConfig();
            }
        }
        else
        {
            ResetConfig();
        }

    }

    public void H2KSyncData()
    {
        if (!enable || !initialized) return;
        // Data synchronization logic here
        "H2KSyncData".LogInfo();
        foreach (var entry in sync_entries)
        {
            entry.H2KSyncData();
        }
    }
    public void H2KSyncData(string hdPath)
    {
        if (!enable || !initialized) return;
        if (hdpath_to_sync_entry.ContainsKey(hdPath))
        {
            ("H2KSyncData2 " + hdPath).LogInfo();
            foreach (var entry in hdpath_to_sync_entry[hdPath])
            {
                entry.H2KSyncData();
            }
        }
    }
    public void LoadCustomizeSyncConfig(string path)
    {
        base_infos = JsonConvert.DeserializeObject<List<SyncBaseInfo>>(File.ReadAllText(path));
        ApplyInfo();
    }
    public void SaveConfig(string path)
    {
        List<SyncBaseInfo> new_info_list = new();
        foreach (var entry in sync_entries)
        {
            new_info_list.Add(entry.ToBaseInfo());
        }
        File.WriteAllText(path, JsonConvert.SerializeObject(new_info_list, Formatting.Indented));
    }
}