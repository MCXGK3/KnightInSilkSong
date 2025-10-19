using Knight;
using KIS;
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