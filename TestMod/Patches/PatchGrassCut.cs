using KIS;

[HarmonyPatch(typeof(GrassCut), "ShouldCut", MethodType.Normal)]
public class Patch_GrassCut_ShouldCut : GeneralPatch
{
    public static bool Prefix(Collider2D collision, ref bool __result)
    {
        if (KnightInSilksong.IsKnight)
        {
            if (!(collision.tag == "Nail Attack") && (!(collision.tag == "HeroBox") || !Knight.HeroController.instance.cState.superDashing))
            {
                __result = collision.tag == "Sharp Shadow";
            }
            else
            {
                __result = true;
            }
            if (!__result)
                return true;
        }
        return false;
    }
    public static void Postfix(Collider2D collision)
    {
    }
}