using HutongGames.PlayMaker.Actions;
using KIS;

[HarmonyPatch(typeof(KnightHatchling), "OnEnable", MethodType.Normal)]
public class Patch_KnightHatchling_OnEnable : GeneralPatch
{
    public static bool Prefix(KnightHatchling __instance)
    {
        if (KnightInSilksong.IsKnight)
        {
            if (__instance.openEffect == null)
            {
                __instance.openEffect = new("openEffect");
                __instance.openEffect.transform.SetParent(__instance.transform);
            }
            // Object.DontDestroyOnLoad(__instance.gameObject);
        }
        return true;
    }
    public static void Postfix(KnightHatchling __instance)
    {
    }
}