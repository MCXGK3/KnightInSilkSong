using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using KIS;
using KIS.Utils;

[HarmonyPatch(typeof(iTweenFsmAction), "OnEnteriTween", typeof(FsmOwnerDefault))]
public class Patch_iTweenFsmAction_OnEnteriTween
{
    public static bool Prefix(iTweenFsmAction __instance)
    {
        return true;
    }
    public static void Postfix(iTweenFsmAction __instance)
    {
        if (KnightInSilksong.IsKnight && __instance.fsm.GetVariable<FsmBool>("FromKnight") != null)
        {
            __instance.realTime = true;
        }
    }
}
[HarmonyPatch(typeof(Wait), "OnEnter", MethodType.Normal)]
public class Patch_Wait_OnEnter
{
    public static bool Prefix(Wait __instance)
    {
        return true;
    }
    public static void Postfix(Wait __instance)
    {
        if (KnightInSilksong.IsKnight && __instance.fsm.GetVariable<FsmBool>("FromKnight") != null)
        {
            if (__instance.time.Value <= 0) return;
            if (__instance.fsm.GameObject.layer == LayerMask.NameToLayer("UI"))
            {
                __instance.realTime = true;
            }
        }
    }
}