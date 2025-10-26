using KIS;

[HarmonyPatch(typeof(GameManager), "PositionHeroAtSceneEntrance", MethodType.Normal)]
public class Patch_GameManager_PositionHeroAtSceneEntrance : GeneralPatch
{
    public static bool Prefix(GameManager __instance)
    {
        return true;
    }
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
    public static bool Prefix(GameManager __instance, bool value)
    {
        return true;
    }
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
        return true;
    }
    public static void Postfix(GameManager __instance)
    {
    }
}