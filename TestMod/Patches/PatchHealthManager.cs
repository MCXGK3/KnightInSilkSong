using KIS;

[HarmonyPatch(typeof(HealthManager), "Hit", new Type[] { typeof(HitInstance) })]
public class Patch_HealthManager_Hit : GeneralPatch
{
    public static bool Prefix(HealthManager __instance, HitInstance hitInstance)
    {
        return true;
    }
    public static void Postfix(HealthManager __instance, HitInstance hitInstance, ref IHitResponder.HitResponse __result)
    {
        if (KnightInSilksong.IsKnight)
            __result.LogInfo();
    }
}
[HarmonyPatch(typeof(HealthManager), "Invincible", new Type[] { typeof(HitInstance) })]
public class Patch_HealthManager_Invincible : GeneralPatch
{
    public static bool Prefix(HealthManager __instance, ref HitInstance hitInstance)
    {
        return true;
    }
    public static void Postfix(HealthManager __instance, ref HitInstance hitInstance)
    {
        if ((((Int32)hitInstance.SpecialType) & KnightInSilksong.KnightDamage) != 0)
        {
            __instance.evasionByHitRemaining = 0.15f;
        }
    }
}

// [HarmonyPatch(typeof(HealthManager), "TakeDamage", new Type[] { typeof(HitInstance) })]
public class Patch_HealthManager_TakeDamage
{
    public static bool Prefix(HealthManager __instance, ref HitInstance hitInstance)
    {
        return true;
    }
    public static void Postfix(HealthManager __instance, ref HitInstance hitInstance)
    {
        if (KnightInSilksong.IsKnight)
            "TakeDamage".LogInfo();
    }
}
[HarmonyPatch(typeof(HealthManager), "NonFatalHit", new Type[] { typeof(bool) })]
public class Patch_HealthManager_NonFatalHit : GeneralPatch
{
    public static bool Prefix(HealthManager __instance, bool ignoreEvasion)
    {
        return true;
    }
    public static void Postfix(HealthManager __instance, bool ignoreEvasion)
    {
        if ((((Int32)__instance.lastHitInstance.SpecialType) & KnightInSilksong.KnightDamage) != 0)
        {
            if (!ignoreEvasion && !__instance.hasAlternateHitAnimation)
            {
                __instance.evasionByHitRemaining = 0.2f;
            }
        }
    }
}