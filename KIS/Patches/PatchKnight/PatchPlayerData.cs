using KIS;
[HarmonyPatch(typeof(Knight.PlayerData), "SetHazardRespawn", new Type[] { typeof(HazardRespawnMarker) })]
public class Patch_PlayerData_SetHazardRespawn2 : GeneralPatch
{
    public static bool Prefix(Knight.PlayerData __instance, HazardRespawnMarker location)
    {
        __instance.hazardRespawnLocation = location.transform.position;
        return false;
    }
}

[HarmonyPatch(typeof(Knight.PlayerData), "TakeHealth", MethodType.Normal)]
public class Patch_PlayerData_TakeHealth : GeneralPatch
{
    public static bool Prefix(Knight.PlayerData __instance, int amount)
    {
        return true;
    }
}

[HarmonyPatch(typeof(Knight.PlayerData), "GetBool", MethodType.Normal)]
public class Patch_Knight_PlayerData_GetBool : GeneralPatch
{
    public static void Postfix(Knight.PlayerData __instance, string boolName, ref bool __result)
    {
        if (boolName == nameof(PlayerData.hasWalljump))
        {
            __result |= !Give_One_WallJump.used_walljump;
            return;
        }
        return;
    }

}

