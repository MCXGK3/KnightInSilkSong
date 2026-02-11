using KIS;

[HarmonyPatch(typeof(GameManager), "PositionHeroAtSceneEntrance", MethodType.Normal)]
public class Patch_GameManager_PositionHeroAtSceneEntrance : GeneralPatch
{
    public static void Postfix(GameManager __instance)
    {
        if (KnightInSilksong.IsKnight)
        {
            Knight.HeroController.instance.transform.position = HeroController.instance.transform.position;
            // Knight.HeroController.instance.RegainControl();
        }
    }
}
[HarmonyPatch(typeof(GameManager), "SetIsInventoryOpen", typeof(bool))]
public class Patch_GameManager_SetIsInventoryOpen : GeneralPatch
{
    public static void Postfix(GameManager __instance, bool value)
    {
        if (KnightInSilksong.IsKnight)
        {
            if (value)
            {
                __instance.SetTimeScale(1f);
                Knight.HeroController.instance.RelinquishControl();
            }
            else
            {
                Knight.HeroController.instance.RegainControl();
            }

        }
    }
}
[HarmonyPatch(typeof(GameManager), "ReturnToMainMenu", MethodType.Normal)]
public class Patch_GameManager_ReturnToMainMenu : GeneralPatch
{
    public static bool Prefix(GameManager __instance)
    {
        if (KnightInSilksong.IsKnight)
        {
            KnightInSilksong.Instance.ToggleKnight();
            KnightInSilksong.return_to_main_menu = true;
            "return to Main Menu".LogInfo();
        }
        KISHelper.OnReturnToMenu?.Invoke();
        return true;
    }
}
[HarmonyPatch(typeof(GameManager), nameof(GameManager.StartNewGame), [typeof(bool), typeof(bool)])]
public class Patch_GameManager_StartNewGame : GeneralPatch
{
    public static bool Prefix(GameManager __instance, bool permadeathMode, bool bossRushMode)
    {
        int save_slot = __instance.profileID;
        if (save_slot == 0) return true;
        KnightInSilksong.Instance.current_data = SlotData.CreateSave(save_slot, KnightInSilksong.default_sync.Value);
        return true;
    }
    public static void Postfix(GameManager __instance, bool permadeathMode, bool bossRushMode)
    {
    }
}
[HarmonyPatch(typeof(GameManager), nameof(GameManager.SetLoadedGameData), [typeof(SaveGameData), typeof(int)])]
public class Patch_GameManager_SetLoadedGameData : GeneralPatch
{
    public static bool Prefix(GameManager __instance, SaveGameData saveGameData, int saveSlot)
    {
        if (saveSlot == 0) return true;
        KnightInSilksong.Instance.current_data = new(saveSlot);
        KnightInSilksong.Instance.current_data.LoadSave();
        return true;
    }
    public static void Postfix(GameManager __instance, SaveGameData saveGameData, int saveSlot)
    {
    }
}
[HarmonyPatch(typeof(GameManager), nameof(GameManager.SaveGame), [typeof(int), typeof(System.Action<bool>), typeof(bool), typeof(AutoSaveName)])]
public class Patch_GameManager_SaveGame : GeneralPatch
{
    public static bool Prefix(GameManager __instance, int saveSlot, ref Action<bool> ogCallback, bool withAutoSave, AutoSaveName autoSaveName)
    {
        if (saveSlot == 0) return true;
        var ogCallbackCopy = ogCallback;
        ogCallback = (didSave) =>
        {
            ogCallbackCopy?.Invoke(didSave);

            if (!didSave)
            {
                return;
            }
            KnightInSilksong.Instance.current_data.SaveSave();
        };
        return true;
    }
}
[HarmonyPatch(typeof(GameManager), nameof(GameManager.ClearSaveFile), MethodType.Normal)]
public class Patch_GameManager_ClearSaveFile : GeneralPatch
{
    public static bool Prefix(GameManager __instance, int saveSlot, Action<bool> callback)
    {
        return true;
    }
    public static void Postfix(GameManager __instance, int saveSlot, Action<bool> callback)
    {
        SlotData.DeleteSave(saveSlot);
    }
}

