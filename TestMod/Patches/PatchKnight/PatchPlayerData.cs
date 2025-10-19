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