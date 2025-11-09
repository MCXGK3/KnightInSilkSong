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
        if (KnightInSilksong.IsKnight)
        {
            var pd = Knight.PlayerData.instance;
            if (pd.GetBool("equippedCharm_10"))
            {
                __instance.details = __instance.dungDetails;
            }
            if (pd.GetBool("equippedCharm_6") && pd.GetInt("health") == 1 && (!pd.GetBool("equippedCharm_27") || pd.GetInt("healthBlue") <= 0))
            {
                if ((bool)(__instance.spriteFlash))
                {
                    __instance.spriteFlash.FlashingFury();
                }
                __instance.details.damage += 5;
            }
            if ((bool)__instance.dungPt)
            {
                if (__instance.details.dung && !__instance.dreamSpawn)
                {
                    __instance.dungPt.Play();
                }
                else
                {
                    __instance.dungPt.Stop();
                }
            }
        }
    }
}