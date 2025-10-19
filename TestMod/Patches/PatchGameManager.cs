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
        if (KnightInSilksong.IsKnight && value)
        {
            __instance.SetTimeScale(1f);
        }
    }
}