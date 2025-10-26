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
        if (KnightInSilksong.IsKnight)
        {
        }
    }
}