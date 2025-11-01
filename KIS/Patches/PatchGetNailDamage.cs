using KIS;

[HarmonyPatch(typeof(GetNailDamage), "OnEnter", MethodType.Normal)]
public class Patch_GetNailDamage_OnEnter : GeneralPatch
{
    public static bool Prefix(GetNailDamage __instance)
    {
        if (KnightInSilksong.IsKnight)
        {
            KnightOnEnter(__instance);
            return false;
        }
        return true;
    }
    public static void Postfix(GetNailDamage __instance)
    {
    }
    public static void KnightOnEnter(GetNailDamage __instance)
    {
        if (!__instance.storeValue.IsNone)
        {
            if (BossSequenceController.BoundNail)
            {
                __instance.storeValue.Value = Mathf.Min(Knight.PlayerData.instance.nailDamage, BossSequenceController.BoundNailDamage);
            }
            else
            {
                __instance.storeValue.Value = Knight.PlayerData.instance.nailDamage;
            }
        }
        
        __instance.Finish();
    }
}