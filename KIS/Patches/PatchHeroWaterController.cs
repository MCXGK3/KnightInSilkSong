using KIS;

[HarmonyPatch(typeof(HeroWaterController), "EnterWaterRegion", MethodType.Normal)]
public class Patch_HeroWaterController_EnterWaterRegion : GeneralPatch
{
    public static bool Prefix(HeroWaterController __instance, SurfaceWaterRegion surfaceWater)
    {
        return true;
    }
    public static void Postfix(HeroWaterController __instance, SurfaceWaterRegion surfaceWater)
    {
    }
}
[HarmonyPatch(typeof(HeroWaterController), "ExitWaterRegion", [typeof(bool)])]
public class Patch_HeroWaterController_ExitWaterRegion : GeneralPatch
{
    public static bool Prefix(HeroWaterController __instance, bool vibrate)
    {
        return true;
    }
}
[HarmonyPatch(typeof(HeroWaterController), "TumbleOut", MethodType.Normal)]
public class Patch_HeroWaterController_TumbleOut : GeneralPatch
{
    public static bool Prefix(HeroWaterController __instance, bool vibrate)
    {
        if (KnightInSilksong.IsKnight)
        {
            return false;
        }
        return true;
    }

}