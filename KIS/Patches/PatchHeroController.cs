using System.Collections;
using GlobalEnums;
using HarmonyLib;
using KIS;

[HarmonyPatch(typeof(HeroController), "RelinquishControl", MethodType.Normal)]
public class Patch_HeroController_RelinquishControl : GeneralPatch
{
    public static HeroController hero;
    public static bool Prefix()
    {
        return true;
    }
    public static void Postfix()
    {
        if (KnightInSilksong.IsKnight)
        {
            Knight.HeroController.instance.RelinquishControl();
        }
    }
}
[HarmonyPatch(typeof(HeroController), "AffectedByGravity", MethodType.Normal)]
public class Patch_HeroController_AffectedByGravity : GeneralPatch
{
    public static bool Prefix(HeroController __instance)
    {
        return true;
    }
    public static void Postfix(HeroController __instance, bool gravityApplies)
    {
        if (KnightInSilksong.IsKnight)
        {
            if (Knight.HeroController.instance.transitionState != HeroTransitionState.WAITING_TO_ENTER_LEVEL && Knight.HeroController.instance.cState.swimming == false)
            {
                if (Knight.HeroController.instance.gameObject.LocateMyFSM("Surface Water").ActiveStateName == "Inactive")
                {
                    Knight.HeroController.instance.AffectedByGravity(gravityApplies);
                    ("AffectedByGravity Patch " + gravityApplies).LogInfo();
                }
            }


        }
    }
}
[HarmonyPatch(typeof(HeroController), "RegainControl", typeof(bool))]
public class Patch_HeroController_RegainControl : GeneralPatch
{
    public static bool Prefix(HeroController __instance, bool allowInput)
    {
        return true;
    }
    public static void Postfix(HeroController __instance, bool allowInput)
    {
        if (KnightInSilksong.IsKnight)
        {
            Knight.HeroController.instance.RegainControl();
        }
    }
}
[HarmonyPatch(typeof(HeroController), "StopAnimationControl", MethodType.Normal)]
public class Patch_HeroController_StopAnimationControl : GeneralPatch
{
    public static bool Prefix(HeroController __instance)
    {
        return true;
    }
    public static void Postfix(HeroController __instance)
    {
        if (KnightInSilksong.IsKnight)
        {
            Knight.HeroController.instance.StopAnimationControl();
        }
    }
}
[HarmonyPatch(typeof(HeroController), "StartAnimationControl", new Type[] { })]
public class Patch_HeroController_StartAnimationControl : GeneralPatch
{
    public static bool Prefix(HeroController __instance)
    {
        return true;
    }
    public static void Postfix(HeroController __instance)
    {
        if (KnightInSilksong.IsKnight)
        {
            Knight.HeroController.instance.StartAnimationControl();

        }
    }
}
[HarmonyPatch(typeof(MonoBehaviour), "StartCoroutine", new Type[] { typeof(IEnumerator) })]
public class Patch_MonoBehaviour_StartCoroutine : GeneralPatch
{
    public static bool Prefix(MonoBehaviour __instance, IEnumerator routine)
    {
        if (__instance == HeroController.instance && KnightInSilksong.IsKnight && !HeroController.instance.gameObject.activeSelf)
        {
            routine.LogInfo();
            Knight.HeroController.instance.StartCoroutine(routine);
            return false;
        }
        return true;
    }
    public static void Postfix(MonoBehaviour __instance)
    {
    }
}
[HarmonyPatch(typeof(HeroController), "LeavingScene", MethodType.Normal)]
public class Patch_HeroController_LeavingScene : GeneralPatch
{
    public static bool Prefix(HeroController __instance)
    {
        return true;
    }
    public static void Postfix(HeroController __instance)
    {
        // if (KnightInSilksong.IsKnight)
        // {
        //     "Leaving Scene Patch".LogInfo();
        //     Knight.HeroController.instance.LeaveScene();
        // }
    }
}

[HarmonyPatch(typeof(HeroController), "LeaveScene", MethodType.Normal)]
public class Patch_HeroController_LeaveScene : GeneralPatch
{
    public static bool Prefix(HeroController __instance, GatePosition? gate)
    {
        if (KnightInSilksong.IsKnight)
        {
            "Leave Scene Patch".LogInfo();
            if (gate == GatePosition.top || gate == GatePosition.bottom)
            {
                gate.LogInfo();
            }
            Knight.HeroController.instance.LeaveScene(gate);
            return false;
        }
        return true;
    }
    public static void Postfix(HeroController __instance, ref GatePosition? gate)
    {

    }
}



[HarmonyPatch(typeof(HeroController), "EnterScene", MethodType.Normal)]
public class Patch_HeroController_EnterScene : GeneralPatch
{
    public static bool Prefix(HeroController __instance, TransitionPoint enterGate, float delayBeforeEnter)
    {
        if (KnightInSilksong.IsKnight)
        {
            if (Knight.HeroController.instance.cState.superDashing)
            {
                Knight.HeroController.instance.SetSuperDashExit();
            }
            if (Knight.HeroController.instance.cState.spellQuake)
            {
                Knight.HeroController.instance.SetQuakeExit();
            }
            GameManager.instance.StartCoroutine(Knight.HeroController.instance.EnterScene(enterGate, delayBeforeEnter));
            enterGate.GetGatePosition().LogInfo();
            "EnterScene Patch".LogInfo();
            PlayMakerFSM.BroadcastEvent("LEVEL LOADED");
            Time.time.LogInfo();

        }
        return true;
    }
    public static void Postfix(HeroController __instance, TransitionPoint enterGate, float delayBeforeEnter)
    {

    }
}
[HarmonyPatch(typeof(HeroController), "EnterSceneDreamGate", MethodType.Normal)]
public class Patch_HeroController_EnterSceneDreamGate : GeneralPatch
{
    public static bool Prefix(HeroController __instance)
    {
        if (KnightInSilksong.IsKnight)
        {
            Knight.HeroController.instance.EnterSceneDreamGate();
            HeroController.instance.gameObject.GetComponent<MeshRenderer>().enabled = false;
            PlayMakerFSM.BroadcastEvent("LEVEL LOADED");
        }
        return true;
    }
    public static void Postfix(HeroController __instance)
    {

    }
}
[HarmonyPatch(typeof(HeroController), "SceneInit", MethodType.Normal)]
public class Patch_HeroController_SceneInit : GeneralPatch
{
    public static bool Prefix(HeroController __instance)
    {
        return true;
    }
    public static void Postfix(HeroController __instance)
    {
        if (KnightInSilksong.IsKnight)
        {
            Knight.HeroController.instance.SceneInit();
        }
    }
}
[HarmonyPatch(typeof(HeroController), "Respawn", MethodType.Normal)]
public class Patch_HeroController_Respawn : GeneralPatch
{
    public static bool Prefix(HeroController __instance)
    {
        return true;
    }
    public static void Postfix(HeroController __instance)
    {
        if (KnightInSilksong.IsKnight)
        {
            GameManager.instance.StartCoroutine(Knight.HeroController.instance.Respawn());
        }
    }
}
[HarmonyPatch(typeof(HeroController), "HazardRespawn", MethodType.Normal)]
public class Patch_HeroController_HazardRespawn : GeneralPatch
{
    public static bool Prefix(HeroController __instance)
    {
        return true;
    }
    public static void Postfix(HeroController __instance)
    {
        if (KnightInSilksong.IsKnight)
        {
            GameManager.instance.StartCoroutine(Knight.HeroController.instance.HazardRespawn());
        }
    }
}
[HarmonyPatch(typeof(HeroController), "MaxHealth", MethodType.Normal)]
public class Patch_HeroController_MaxHealth : GeneralPatch
{
    public static bool Prefix(HeroController __instance)
    {
        return true;
    }
    public static void Postfix(HeroController __instance)
    {
        if (KnightInSilksong.IsKnight)
        {
            Knight.HeroController.instance.MaxHealth();
            EventRegister.SendEvent("UPDATE BLUE HEALTH");
        }
    }
}
[HarmonyPatch(typeof(HeroController), "AddSilk", new Type[] { typeof(int), typeof(bool), typeof(SilkSpool.SilkAddSource), typeof(bool) })]
public class Patch_HeroController_AddSilk : GeneralPatch
{
    public static bool Prefix(HeroController __instance, int amount, bool heroEffect, SilkSpool.SilkAddSource source, bool forceCanBindEffect)
    {
        return true;
    }
    public static void Postfix(HeroController __instance, int amount, bool heroEffect, SilkSpool.SilkAddSource source, bool forceCanBindEffect)
    {
        if (KnightInSilksong.IsKnight)
        {
            Knight.HeroController.instance.SoulGain();
        }
    }
}
[HarmonyPatch(typeof(HeroController), "IgnoreInput", MethodType.Normal)]
public class Patch_HeroController_IgnoreInput : GeneralPatch
{
    public static bool Prefix(HeroController __instance)
    {
        return true;
    }
    public static void Postfix(HeroController __instance)
    {
        if (KnightInSilksong.IsKnight)
        {
            Knight.HeroController.instance.IgnoreInput();
        }
    }
}
[HarmonyPatch(typeof(HeroController), "IgnoreInputWithoutReset", MethodType.Normal)]
public class Patch_HeroController_IgnoreInputWithoutReset : GeneralPatch
{
    public static bool Prefix(HeroController __instance)
    {
        return true;
    }
    public static void Postfix(HeroController __instance)
    {
        if (KnightInSilksong.IsKnight)
        {
            Knight.HeroController.instance.IgnoreInputWithoutReset();
        }
    }
}
[HarmonyPatch(typeof(HeroController), "AcceptInput", MethodType.Normal)]
public class Patch_HeroController_AcceptInput : GeneralPatch
{
    public static bool Prefix(HeroController __instance)
    {
        return true;
    }
    public static void Postfix(HeroController __instance)
    {
        if (KnightInSilksong.IsKnight)
        {
            Knight.HeroController.instance.AcceptInput();
        }
    }
}
[HarmonyPatch(typeof(HeroController), "Pause", MethodType.Normal)]
public class Patch_HeroController_Pause : GeneralPatch
{
    public static bool Prefix(HeroController __instance)
    {
        return true;
    }
    public static void Postfix(HeroController __instance)
    {
        if (KnightInSilksong.IsKnight)
        {
            Knight.HeroController.instance.Pause();
        }
    }
}
[HarmonyPatch(typeof(HeroController), "UnPause", MethodType.Normal)]
public class Patch_HeroController_UnPause : GeneralPatch
{
    public static bool Prefix(HeroController __instance)
    {
        return true;
    }
    public static void Postfix(HeroController __instance)
    {
        if (KnightInSilksong.IsKnight)
        {
            Knight.HeroController.instance.UnPause();
        }
    }
}
[HarmonyPatch(typeof(HeroController), "RecoilRight", MethodType.Normal)]
public class Patch_HeroController_RecoilRight : GeneralPatch
{
    public static bool Prefix(HeroController __instance)
    {
        if (KnightInSilksong.IsKnight)
        {
            Knight.HeroController.instance.RecoilRight();
            return false;
        }
        return true;
    }
    public static void Postfix(HeroController __instance)
    {
    }
}
[HarmonyPatch(typeof(HeroController))]
[HarmonyPatch("RecoilRightLong")]
public class Patch_HeroController_RecoilRightLong : GeneralPatch
{
    static bool Prefix()
    {
        if (KnightInSilksong.IsKnight)
        {
            Knight.HeroController.instance.RecoilRightLong();
            return false;
        }
        return true;
    }
    public static void Postfix() {}
}
[HarmonyPatch(typeof(HeroController), "RecoilLeft", MethodType.Normal)]
public class Patch_HeroController_RecoilLeft : GeneralPatch
{
    public static bool Prefix(HeroController __instance)
    {
        if (KnightInSilksong.IsKnight)
        {
            Knight.HeroController.instance.RecoilLeft();
            return false;
        }
        return true;
    }
    public static void Postfix(HeroController __instance)
    {
    }
}
[HarmonyPatch(typeof(HeroController), "RecoilLeftLong", MethodType.Normal)]
public class Patch_HeroController_RecoilLeftLong : GeneralPatch
{
    public static bool Prefix(HeroController __instance)
    {
        if (KnightInSilksong.IsKnight)
        {
            Knight.HeroController.instance.RecoilLeftLong();
            return false;
        }
        return true;
    }
    public static void Postfix(HeroController __instance)
    {
    }
}
[HarmonyPatch(typeof(HeroController), "RecoilDown", MethodType.Normal)]
public class Patch_HeroController_RecoilDown : GeneralPatch
{
    public static bool Prefix(HeroController __instance)
    {
        if (KnightInSilksong.IsKnight)
        {
            Knight.HeroController.instance.RecoilDown();
            return false;
        }
        return true;
    }
    public static void Postfix(HeroController __instance)
    {
    }
}
[HarmonyPatch(typeof(HeroController), "DownspikeBounce", MethodType.Normal)]
public class Patch_HeroController_DownspikeBounce : GeneralPatch
{
    public static bool Prefix(HeroController __instance, bool harpoonRecoil, HeroSlashBounceConfig bounceConfig = null)
    {
        if (KnightInSilksong.IsKnight)
        {
            Traverse.Create(Knight.HeroController.instance).Method("CancelBounce").GetValue();
            "ShroomBounce".LogInfo();

            Rigidbody2D rb2d = Knight.HeroController.instance.GetComponent<Rigidbody2D>();
            if (rb2d.linearVelocity.y == Knight.HeroController.instance.SHROOM_BOUNCE_VELOCITY)
            {
                // attempting to detect a shroombounce and not override it
                // maybe not the best way to do so but this works
                Knight.HeroController.instance.ShroomBounce();
                return false;
            }
            Knight.HeroController.instance.Bounce();
            // Knight.HeroController.instance.ShroomBounce();//shroom or general bounce?
            return false;
        }
        return true;
    }
    public static void Postfix(HeroController __instance, bool harpoonRecoil, HeroSlashBounceConfig bounceConfig = null)
    {

    }
}
[HarmonyPatch(typeof(HeroController), "FinishedEnteringScene", MethodType.Normal)]
public class Patch_HeroController_FinishedEnteringScene : GeneralPatch
{
    public static bool Prefix(HeroController __instance)
    {
        return true;
    }
    public static void Postfix(HeroController __instance)
    {
        "Hornet FinishedEnteringScene Patch".LogInfo();
        Time.time.LogInfo();
    }
}
[HarmonyPatch(typeof(HeroController), "Awake", MethodType.Normal)]
public class Patch_HeroController_Awake : GeneralPatch
{
    public static bool Prefix(HeroController __instance)
    {
        KnightInSilksong.Instance.InstKnight();
        Knight.HeroController.instance.gameObject.SetActive(false);
        "Create Knight".LogInfo();
        return true;
    }
    public static void Postfix(HeroController __instance)
    {

    }
}
[HarmonyPatch(typeof(HeroController), "OnDestroy", MethodType.Normal)]
public class Patch_HeroController_OnDestroy : GeneralPatch
{
    public static bool Prefix(HeroController __instance)
    {
        return true;
    }
    public static void Postfix(HeroController __instance)
    {
        GameObject.DestroyImmediate(Knight.HeroController.instance.gameObject);
        "Destroy Knight".LogInfo();
    }
}
[HarmonyPatch(typeof(HeroController), "IsSwimming", MethodType.Normal)]
public class Patch_HeroController_IsSwimming : GeneralPatch
{
    public static bool Prefix(HeroController __instance)
    {
        return true;
    }
    public static void Postfix(HeroController __instance)
    {
        if (KnightInSilksong.IsKnight)
        {

            "IsSwimming Patch".LogInfo();
        }
    }
}
[HarmonyPatch(typeof(HeroController), "NotSwimming", MethodType.Normal)]
public class Patch_HeroController_NotSwimming : GeneralPatch
{
    public static bool Prefix(HeroController __instance)
    {
        return true;
    }
    public static void Postfix(HeroController __instance)
    {
        if (KnightInSilksong.IsKnight)
        {

        }
    }
}
[HarmonyPatch(typeof(HeroController), "StartAnimationControlToIdle", MethodType.Normal)]
public class Patch_HeroController_StartAnimationControlToIdle : GeneralPatch
{
    public static bool Prefix(HeroController __instance)
    {
        return true;
    }
    public static void Postfix(HeroController __instance)
    {
        if (KnightInSilksong.IsKnight)
        {
            Knight.HeroController.instance.StartAnimationControl();
        }
    }
}

[HarmonyPatch(typeof(HeroController), "StartAnimationControlToIdleForcePlay", MethodType.Normal)]
public class Patch_HeroController_StartAnimationControlToIdleForcePlay : GeneralPatch
{
    public static bool Prefix(HeroController __instance)
    {
        return true;
    }
    public static void Postfix(HeroController __instance)
    {
        if (KnightInSilksong.IsKnight)
        {
            Knight.HeroController.instance.StartAnimationControl();
        }
    }
}
[HarmonyPatch(typeof(HeroController), "CanTakeDamage", MethodType.Normal)]
public class Patch_HeroController_CanTakeDamage : GeneralPatch
{
    public static bool Prefix(HeroController __instance, ref bool __result)
    {
        if (KnightInSilksong.IsKnight)
        {
            bool result = false;
            result |= Traverse.Create(Knight.HeroController.instance).Method("CanTakeDamage").GetValue<bool>();
            __result = result;
            return false;
        }
        return true;
    }
    public static void Postfix(HeroController __instance)
    {
    }
}
[HarmonyPatch(typeof(HeroController), "NailParry", MethodType.Normal)]
public class Patch_HeroController_NailParry : GeneralPatch
{
    public static bool Prefix(HeroController __instance)
    {
        if (KnightInSilksong.IsKnight)
        {
            Knight.HeroController.instance.NailParry();
            return false;
        }
        return true;
    }
    public static void Postfix(HeroController __instance)
    {

    }
}
[HarmonyPatch(typeof(HeroController), "NailParryRecover", MethodType.Normal)]
public class Patch_HeroController_NailParryRecover : GeneralPatch
{
    public static bool Prefix(HeroController __instance)
    {
        if (KnightInSilksong.IsKnight)
        {
            Knight.HeroController.instance.NailParryRecover();
            return false;
        }
        return true;
    }
    public static void Postfix(HeroController __instance)
    {

    }
}



