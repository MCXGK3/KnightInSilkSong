using HutongGames.PlayMaker.Actions;
using KIS;

[HarmonyPatch(typeof(GetHeroCState), "DoAction", MethodType.Normal)]
public class Patch_GetHeroCState_DoAction : GeneralPatch
{
    public static bool Prefix(GetHeroCState __instance)
    {
        if (KnightInSilksong.IsKnight)
        {
            if (!__instance.VariableName.IsNone && !__instance.StoreValue.IsNone)
            {
                if (__instance.VariableName.Value == "parrying")
                {
                    bool res = false;
                    var hc = Knight.HeroController.instance;
                    res = hc.playerData.equippedCharm_5 && hc.playerData.blockerHits > 0 && hc.cState.focusing;
                    __instance.StoreValue.Value = res;
                    return false;
                }
            }
        }

        return true;
    }
    public static void Postfix(GetHeroCState __instance)
    {
    }
}