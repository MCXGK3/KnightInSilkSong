using KIS;
using Knight;
[HarmonyPatch(typeof(Knight.PlayerData), "SetHazardRespawn", new Type[] { typeof(HazardRespawnMarker) })]
public class Patch_PlayerData_SetHazardRespawn2 : GeneralPatch
{
    public static bool Prefix(Knight.PlayerData __instance, HazardRespawnMarker location)
    {
        __instance.hazardRespawnLocation = location.transform.position;
        return false;
    }
    public static void Postfix(Knight.PlayerData __instance)
    {
    }
}
[HarmonyPatch(typeof(Knight.PlayerData), "TakeHealth", MethodType.Normal)]
public class Patch_PlayerData_TakeHealth : GeneralPatch
{
    public static bool Prefix(Knight.PlayerData __instance, int amount)
    {
        return true;
    }
    public static void Postfix(Knight.PlayerData __instance, int amount)
    {
    }
}
[HarmonyPatch(typeof(Knight.PlayerData), "StartSoulLimiter", MethodType.Normal)]
public class Patch_Knight_PlayerData_StartSoulLimiter : GeneralPatch
{
    public static bool Prefix(Knight.PlayerData __instance)
    {
        if (KnightInSilksong.IsKnight)
        {
            "try start soul limiter".LogInfo();
            return false;
        }
        return true;
    }
    public static void Postfix(Knight.PlayerData __instance)
    {
    }
}