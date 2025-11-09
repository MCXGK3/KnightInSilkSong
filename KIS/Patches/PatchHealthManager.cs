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
[HarmonyPatch(typeof(HealthManager), "IsBlockingByDirection", MethodType.Normal)]
public class Patch_HealthManager_IsBlockingByDirection : GeneralPatch
{
    public static bool Prefix(HealthManager __instance, int cardinalDirection, AttackTypes attackType, ref SpecialTypes specialType)
    {
        return true;
    }
    public static void Postfix(HealthManager __instance, int cardinalDirection, AttackTypes attackType, SpecialTypes specialType, ref bool __result)
    {
    }
}
[HarmonyPatch(typeof(HealthManager), "IsInvincible", MethodType.Setter)]
public class Patch_HealthManager_IsInvincible : GeneralPatch
{
    public static bool Prefix(HealthManager __instance, bool value)
    {
        return true;
    }
    public static void Postfix(HealthManager __instance)
    {
    }
}
[HarmonyPatch(typeof(HealthManager), "InvincibleFromDirection", MethodType.Setter)]
public class Patch_HealthManager_InvincibleFromDirection : GeneralPatch
{
    public static bool Prefix(HealthManager __instance, int value)
    {
        return true;
    }
    public static void Postfix(HealthManager __instance)
    {
    }
}

[HarmonyPatch(typeof(HealthManager), "ApplyDamageScaling", MethodType.Normal)]
public class Patch_HealthManager_ApplyDamageScaling : GeneralPatch
{
    public static bool Prefix(HealthManager __instance, ref HitInstance hitInstance, ref HitInstance __result)
    {

        if (KnightInSilksong.IsKnight)
        {
            if (KnightInSilksong.apply_damage_scaling.Value == false) return true;
            if ((((int)hitInstance.SpecialType) & KnightInSilksong.KnightDamage) != 0)
            {
                int level = (Knight.PlayerData.instance.nailDamage / 4) - 1;
                float multFromLevel = __instance.damageScaling.GetMultFromLevel(level);
                hitInstance.DamageDealt = Mathf.RoundToInt((float)hitInstance.DamageDealt * multFromLevel);
                __result = hitInstance;
                return false;
            }
        }
        return true;
    }
    public static void Postfix(HealthManager __instance, HitInstance hitInstance)
    {
    }
}