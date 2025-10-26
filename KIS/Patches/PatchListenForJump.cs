using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using KIS;
using KIS.Utils;

[HarmonyPatch(typeof(ListenForJump), "OnEnter", MethodType.Normal)]
public class Patch_ListenForJump_OnEnter : GeneralPatch
{
    public static bool Prefix(ListenForJump __instance)
    {
        if (KnightInSilksong.IsKnight)
        {
            if (__instance.fsm.GetVariable<FsmBool>("FromKnight") != null)
            {
                __instance.activeBool = true;
                // "ListenForJump OnEnter Prefix".LogInfo();
            }
        }
        return true;
    }
    public static void Postfix(ListenForJump __instance)
    {
    }
}
[HarmonyPatch(typeof(ListenForJump), "CheckInput", MethodType.Normal)]
public class Patch_ListenForJump_CheckInput : GeneralPatch
{
    public static bool Prefix(ListenForJump __instance)
    {
        if (KnightInSilksong.IsKnight)
        {
            if (__instance.fsm.GetVariable<FsmBool>("FromKnight") != null)
            {
                CheckInput(__instance);
                // "ListenForJump CheckInput Prefix".LogInfo();
                return false;
            }
        }
        return true;
    }
    public static void Postfix(ListenForJump __instance)
    {
    }
    private static void CheckInput(ListenForJump __instance)
    {
        if (GameManager.instance.isPaused || (!__instance.activeBool.IsNone && !__instance.activeBool.Value))
        {
            return;
        }

        if (__instance.inputHandler.inputActions.Jump.WasPressed)
        {
            "WAS PRESSED1".LogInfo();
            __instance.Fsm.Event(__instance.wasPressed);
            "WAS PRESSED2".LogInfo();
            if (!__instance.queueBool.IsNone)
            {
                __instance.queueBool.Value = true;
            }
        }

        if (__instance.inputHandler.inputActions.Jump.WasReleased)
        {
            __instance.Fsm.Event(__instance.wasReleased);
        }

        if (__instance.inputHandler.inputActions.Jump.IsPressed)
        {
            __instance.Fsm.Event(__instance.isPressed);
        }

        if (!__instance.inputHandler.inputActions.Jump.IsPressed)
        {
            __instance.Fsm.Event(__instance.isNotPressed);
            if (!__instance.queueBool.IsNone)
            {
                __instance.queueBool.Value = false;
            }
        }
    }
}