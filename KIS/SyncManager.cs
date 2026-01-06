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
    private SyncBaseInfo Tool2Charm(Tool tool, Charm charm, bool always_equal = true)
    {
        return new(FromTool.prefix + tool.GetToolName(), true, charm.GetCharmName(), true, always_equal);
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
            Tool2Charm(Tool.Compass,Charm.WaywardCompass),
            Tool2Charm(Tool.Sting_Shard,Charm.Weaversong),
            Tool2Charm(Tool.Pimpilo,Charm.DefendersCrest),
            Tool2Charm(Tool.Lightning_Rod,Charm.MarkOfPride),
            Tool2Charm(Tool.Flintstone,Charm.UnbreakableStrength),
            Tool2Charm(Tool.Flea_Brew,Charm.QuickSlash),
            Tool2Charm(Tool.Lifeblood_Syringe,Charm.JonisBlessing),
            DefaultValue(Charm.Grubsong.GetCharmName(),false),
            Tool2Charm(Tool.Mosscreep_Tool_1,Charm.Grubsong,false),
            Tool2Charm(Tool.Mosscreep_Tool_2,Charm.Grubsong,false),
            Tool2Charm(Tool.Bell_Bind,Charm.BaldurShell),
            Tool2Charm(Tool.Poison_Pouch,Charm.Flukenest),
            Tool2Charm(Tool.Lava_Charm,Charm.StalwartShell),
            Tool2Charm(Tool.Fractured_Mask,Charm.LifebloodHeart),
            Tool2Charm(Tool.Multibind,Charm.DeepFocus),
            Tool2Charm(Tool.White_Ring,Charm.SoulEater),
            Tool2Charm(Tool.Brolly_Spike,Charm.SharpShadow),
            Tool2Charm(Tool.Quickbind,Charm.QuickFocus),
            Tool2Charm(Tool.Spool_Extender,Charm.SpellTwister),
            Tool2Charm(Tool.Reserve_Bind,Charm.Hiveblood),
            DefaultValue(Charm.SporeShroom.GetCharmName(),false),
            Tool2Charm(Tool.Dazzle_Bind,Charm.SporeShroom),
            Tool2Charm(Tool.Dazzle_Bind_Upgraded,Charm.SporeShroom),
            Tool2Charm(Tool.Revenge_Crystal,Charm.ThornsOfAgony),
            Tool2Charm(Tool.Zap_Imbuement,Charm.ShamanStone),
            Tool2Charm(Tool.Quick_Sling,Charm.SoulCatcher),
            Tool2Charm(Tool.Maggot_Charm,Charm.LifebloodCore),
            Tool2Charm(Tool.Longneedle,Charm.Longnail),
            Tool2Charm(Tool.Wisp_Lantern,Charm.GlowingWomb),
            Tool2Charm(Tool.Flea_Charm,Charm.GrubberflysElegy),
            Tool2Charm(Tool.Pinstress_Tool,Charm.NailmastersGlory),
            Tool2Charm(Tool.Bone_Necklace,Charm.HeavyBlow),
            Tool2Charm(Tool.Rosary_Magnet,Charm.GatheringSwarm),
            Tool2Charm(Tool.Weighted_Anklet,Charm.SteadyBody),
            Tool2Charm(Tool.Barbed_Wire,Charm.FuryOfTheFallen),
            DefaultValue(Charm.UnbreakableHeart.GetCharmName(),false),
            Tool2Charm(Tool.Dead_Mans_Purse,Charm.UnbreakableHeart,false),
            Tool2Charm(Tool.Shell_Satchel,Charm.UnbreakableHeart,false),
            DefaultValue(Charm.Grimmchild.GetCharmName(),false),
            DefaultValue(nameof(kd.grimmChildLevel),4),
            Tool2Charm(Tool.Cogwork_Flier,Charm.Grimmchild,false),
            // new(FromTool.prefix+Tool.Cogwork_Flier.GetToolName(),true,nameof(kd.grimmChildLevel),4),
            Tool2Charm(Tool.Magnetite_Dice,Charm.Grimmchild,false),
            new(FromTool.prefix+Tool.Magnetite_Dice.GetToolName(),true,nameof(kd.grimmChildLevel),5,false),
            Tool2Charm(Tool.Scuttlebrace,Charm.Dashmaster),
            Tool2Charm(Tool.Sprintmaster,Charm.Sprintmaster),
            Tool2Charm(Tool.Musician_Charm,Charm.DreamWielder),
            Tool2Charm(Tool.Thief_Charm,Charm.UnbreakableGreed),
            Tool2Charm(Tool.Wallcling,Charm.ShapeOfUnn),
            Tool2Charm(Tool.Cogwork_Saw,Charm.Dreamshield)


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