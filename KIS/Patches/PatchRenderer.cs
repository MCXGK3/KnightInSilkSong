using KIS;

[HarmonyPatch(typeof(Renderer), "enabled", MethodType.Setter)]
public class Patch_Renderer_enabled : GeneralPatch
{
    public static bool Prefix(Renderer __instance, bool value)
    {
        if (HeroController.instance != null && __instance.gameObject == HeroController.instance.gameObject)
        {
            if (KnightInSilksong.IsKnight)
            {
                return !value;
            }
        }
        return true;
    }
    public static void Postfix(Renderer __instance)
    {

    }
}