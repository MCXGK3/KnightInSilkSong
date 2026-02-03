using KIS;

[HarmonyPatch(typeof(BounceBalloon), "Bounce", MethodType.Enumerator)]
public class Patch_BounceBallon_Bounce : GeneralPatch
{
    public static bool Prefix()
    {
        if (KnightInSilksong.IsKnight)
        {
            Knight.HeroController.instance.ShroomBounce();
            return false;
        }
        return true;
    }
}
