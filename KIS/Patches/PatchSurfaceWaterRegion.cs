using KIS;

[HarmonyPatch(typeof(SurfaceWaterRegion), "OnTriggerEnter2D", MethodType.Normal)]
public class Patch_SurfaceWaterRegion_OnTriggerEnter2D : GeneralPatch
{
    public static bool Prefix(SurfaceWaterRegion __instance, Collider2D collision)
    {
        if (KnightInSilksong.IsKnight && collision.gameObject == Knight.HeroController.instance.gameObject)
        {
            Knight.HeroController.instance.gameObject.LocateMyFSM("Surface Water").SendEvent("SURFACE ENTER");
            "SurfaceWaterRegion OnTriggerEnter2D Prefix".LogInfo();
            return false;
        }
        return true;
    }
    public static void Postfix(SurfaceWaterRegion __instance)
    {
    }
}