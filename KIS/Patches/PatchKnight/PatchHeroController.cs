using Knight;
using KIS;
using GlobalEnums;
[HarmonyPatch(typeof(Knight.HeroController), "LocateSpawnPoint", MethodType.Normal)]
public class Patch_HeroController_LocateSpawnPoint : GeneralPatch
{
    public static bool Prefix(Knight.HeroController __instance, ref Transform __result)
    {
        if (KnightInSilksong.IsKnight)
        {
            __result = global::HeroController.instance.LocateSpawnPoint();
            return false;
        }
        return true;
    }
    public static void Postfix(Knight.HeroController __instance)
    {
    }
}
[HarmonyPatch(typeof(Knight.HeroController), "CharmUpdate", MethodType.Normal)]
public class Patch_HeroController_CharmUpdate : GeneralPatch
{
    public static bool Prefix(Knight.HeroController __instance)
    {
        return true;
    }
    public static void Postfix(Knight.HeroController __instance)
    {
        if (KnightInSilksong.IsKnight)
        {
            PlayMakerFSM.BroadcastEvent("CHARM EQUIP CHECK");
            PlayMakerFSM.BroadcastEvent("CHARM INDICATOR CHECK");
            EventRegister.SendEvent("UPDATE BLUE HEALTH");
        }
    }
}
[HarmonyPatch(typeof(Knight.HeroController), "Attack", MethodType.Normal)]
public class Patch_HeroController_Attack : GeneralPatch
{
    public static bool Prefix(Knight.HeroController __instance, AttackDirection attackDir)
    {
        if (KnightInSilksong.IsKnight)
        {
            HeroController.instance.IncrementAttackCounter();
        }
        return true;
    }
    public static void Postfix(Knight.HeroController __instance, AttackDirection attackDir)
    {
    }
}

[HarmonyPatch(typeof(Knight.HeroController), "FinishedEnteringScene", MethodType.Normal)]
public class Patch_Knight_HeroController_FinishedEnteringScene : GeneralPatch
{
    public static bool Prefix()
    {
        return true;
    }
    public static void Postfix()
    {
        "Knight FinishedEnteringScene Patch".LogInfo();
        Time.time.LogInfo();
    }
}

[HarmonyPatch(typeof(Knight.HeroController), "TakeDamage", MethodType.Normal)]
public class Patch_Knight_HeroController_TakeDamage : GeneralPatch
{
    public static bool Prefix(Knight.HeroController __instance, GameObject go, CollisionSide damageSide, int damageAmount, ref int hazardType)
    {
        if (KnightInSilksong.IsKnight)
        {
            if (hazardType == (int)HazardType.LAVA)
            {
                hazardType = (int)HazardType.PIT;
            }
            else if (hazardType == KnightInSilksong.HazardType_NORESPOND)
            {
                hazardType = (int)HazardType.LAVA;
            }
        }
        return true;
    }
    public static void Postfix(Knight.HeroController __instance, GameObject go, CollisionSide damageSide, int damageAmount, int hazardType)
    {
    }
}
