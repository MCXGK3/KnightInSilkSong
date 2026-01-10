using System.Collections;
using HutongGames.PlayMaker.Actions;

[HarmonyPatch(typeof(RespondToCurrencyCounterEvents), "OnEnter", MethodType.Normal)]
public class Patch_RespondToCurrencyCounterEvents_OnEnter : GeneralPatch
{
    public static bool Prefix(RespondToCurrencyCounterEvents __instance)
    {
        __instance.fsm.FsmComponent.StartCoroutine(DelayFinished(__instance));
        return true;
    }
    public static void Postfix(RespondToCurrencyCounterEvents __instance)
    {
    }
    static IEnumerator DelayFinished(RespondToCurrencyCounterEvents __instance)
    {
        yield return new WaitForSeconds(0.5f);
        if (__instance.State.active)
        {
            __instance.Event(__instance.Response);
        }
    }
}