using KIS;

[HarmonyPatch(typeof(HeroAnimationController), "StartControl", MethodType.Normal)]
public class Patch_HeroAnimationController_StartControl : GeneralPatch
{
    public static bool Prefix(HeroAnimationController __instance)
    {
        return true;
    }
    public static void Postfix(HeroAnimationController __instance)
    {
        if (KnightInSilksong.IsKnight)
        {
            Knight.HeroController.instance.GetComponent<Knight.HeroAnimationController>().StartControl();
        }
    }
}
[HarmonyPatch(typeof(HeroAnimationController), "StartControlToIdle", [typeof(bool)])]
public class Patch_HeroAnimationController_StartControlToIdle : GeneralPatch
{
    public static bool Prefix(HeroAnimationController __instance, bool forcePlay)
    {
        return true;
    }
    public static void Postfix(HeroAnimationController __instance, bool forcePlay)
    {
        if (KnightInSilksong.IsKnight)
        {
            Knight.HeroController.instance.GetComponent<Knight.HeroAnimationController>().StartControl();
        }
    }
}
[HarmonyPatch(typeof(HeroAnimationController), "StopControl", MethodType.Normal)]
public class Patch_HeroAnimationController_StopControl : GeneralPatch
{
    public static bool Prefix(HeroAnimationController __instance)
    {
        return true;
    }
    public static void Postfix(HeroAnimationController __instance)
    {
        if (KnightInSilksong.IsKnight)
        {
            Knight.HeroController.instance.GetComponent<Knight.HeroAnimationController>().StopControl();
        }
    }
}