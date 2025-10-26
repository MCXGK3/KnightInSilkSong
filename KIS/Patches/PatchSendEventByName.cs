
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using KIS;
using KIS.Utils;

[HarmonyPatch(typeof(SendEventByName), "OnEnter", MethodType.Normal)]
public class Patch_SendEventByName_OnEnter //: GeneralPatch
{
    public static bool Prefix(SendEventByName __instance)
    {
        if (KnightInSilksong.IsKnight)
        {
            if (__instance.sendEvent.value == "UPDATE BLUE HEALTH" && __instance.fsm.GetVariable<FsmBool>("FromKnight") != null)
            {
                (__instance.fsm.GameObject.name + " " + __instance.fsm.name + " " + __instance.State.name + " SendEventByName UPDATE BLUE HEALTH").LogInfo();
                __instance.Finish();
                return false;
            }
        }
        return true;
    }
    public static void Postfix(SendEventByName __instance)
    {
        if (KnightInSilksong.IsKnight)
        {
            if (__instance.sendEvent.value == "REMOVE BLUE HEALTH")
            {
                (__instance.fsm.GameObject.name + " " + __instance.fsm.name + " " + __instance.State.name + " SendEventByName REMOVE BLUE HEALTH").LogInfo();
            }
            if (__instance.sendEvent.value == "UPDATE BLUE HEALTH")
            {
                (__instance.fsm.GameObject.name + " " + __instance.fsm.name + " " + __instance.State.name + " SendEventByName UPDATE BLUE HEALTH").LogInfo();
            }
        }
    }
}