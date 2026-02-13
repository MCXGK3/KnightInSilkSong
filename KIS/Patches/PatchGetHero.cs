using KIS;

[HarmonyPatch(typeof(GetHero), nameof(GetHero.OnEnter), MethodType.Normal)]
public class Patch_GetHero_OnEnter : GeneralPatch
{
    public static HashSet<(string, string)> blacklist = [
        ("RestBench","Bench Control")
    ];
    public static bool Prefix(GetHero __instance)
    {
        return true;
        if (blacklist.Contains((__instance.fsm.GameObjectName, __instance.fsm.name)))
        {
            return true;
        }
        if (KnightInSilksong.IsKnight)
        {
            __instance.storeResult.Value = KISHelper.GetCurrentHero();
            __instance.Finish();
            return false;
        }
        return true;
    }
    public static void Postfix(GetHero __instance)
    {
    }
}