using System.IO;
using BepInEx;
using BepInEx.Configuration;
using HutongGames.PlayMaker.Actions;
using KIS;
using Newtonsoft.Json;
using static KIS.SyncManager;

public class SlotData
{
    const string general = "General";
    const string gameplay = "GamePlay";
    string slot_setting_path => Path.Combine(KISHelper.GetSaveDataDirectory(slot.Value), "SlotSetting.cfg");
    string playerdata_path => Path.Combine(KISHelper.GetSaveDataDirectory(slot.Value), "PlayerData.json");
    string sync_config_path => Path.Combine(KISHelper.GetSaveDataDirectory(slot.Value), "SyncConfig.json");
    public SlotData(int slot)
    {
        this.slot = slot;
        slot_file = new(slot_setting_path, true);
        sync = slot_file.Bind<bool>(general, "Sync", false, "make knight sync with hornet in this save");
        once_walljump = slot_file.Bind<bool>(gameplay, "OnceWallJump", true, "Use the one-time-use walljump instead of wall scrambling");
        inf_jump_in_wind = slot_file.Bind<bool>(gameplay, "InfJumpInWind", true, "Jump infinitely in air columns instead of mantle");
        pogoable_ring = slot_file.Bind<bool>(gameplay, "PogoableRing", true, "Make ring pogoable to replace the harpoon");
        upward_superdash = slot_file.Bind<bool>(gameplay, "UpwardSuperDash", true, "Use upward superdash instead of upward harpoon");
    }
    int? slot = null;
    ConfigFile slot_file;
    ConfigEntry<bool> sync;
    ConfigEntry<bool> once_walljump;
    ConfigEntry<bool> inf_jump_in_wind;
    ConfigEntry<bool> pogoable_ring;
    ConfigEntry<bool> upward_superdash;
    List<SyncBaseInfo> baseInfos;
    Knight.PlayerData playerData;
    private void Setting2Default()
    {
        foreach (var set in slot_file)
        {
            set.Value.BoxedValue = set.Value.DefaultValue;
        }
    }
    public static void DeleteSave(int slot)
    {
        SlotData slotData = new(slot);
        slotData.Setting2Default();
        slotData.playerData = null;
        if (File.Exists(slotData.playerdata_path))
        {
            File.Delete(slotData.playerdata_path);
        }
    }
    public static SlotData CreateSave(int slot, bool sync_value)
    {

        SlotData slot_data = new(slot);
        slot_data.sync.Value = sync_value;
        if (File.Exists(slot_data.playerdata_path))
        {
            File.Delete(slot_data.playerdata_path);
        }
        slot_data.LoadSyncConfig();
        slot_data.playerData = new();
        if (!slot_data.sync.Value)
        {
            slot_data.playerData.AddGGPlayerDataOverrides();
            slot_data.playerData.royalCharmState = 4;
        }
        return slot_data;
    }
    public void LoadSave()
    {
        bool flag = false;
        if (File.Exists(playerdata_path) && playerData == null)
        {
            try
            {
                playerData = new();
                if (!sync.Value)
                {
                    playerData.AddGGPlayerDataOverrides();
                    playerData.royalCharmState = 4;
                }
                JsonUtility.FromJsonOverwrite(File.ReadAllText(playerdata_path), playerData);
                flag = true;
            }
            catch (Exception e)
            {
                ("Loading PlayerData Error: " + e).LogWarning();
                flag = false;
            }
        }
        if (!flag)
        {
            playerData = new();
            if (!sync.Value)
            {
                playerData.AddGGPlayerDataOverrides();
                playerData.royalCharmState = 4;
            }
        }
        Knight.PlayerData.instance = playerData;
        LoadSyncConfig();
        SyncManager.Instance.Initialize(baseInfos, sync.Value);
    }
    public void SaveSave()
    {
        if (playerData != null)
        {
            File.WriteAllText(playerdata_path, JsonUtility.ToJson(playerData, true));
        }
    }
    public void SaveSyncConfig()
    {
        if (baseInfos != null)
        {
            File.WriteAllText(sync_config_path, JsonConvert.SerializeObject(baseInfos, Formatting.Indented));
        }
    }
    public void LoadSyncConfig()
    {
        if (File.Exists(sync_config_path))
        {
            try
            {
                baseInfos = JsonConvert.DeserializeObject<List<SyncBaseInfo>>(sync_config_path);
                return;
            }
            catch (Exception e)
            {
                e.LogWarning();
            }
        }
        baseInfos = DefaultConfig();

    }
}