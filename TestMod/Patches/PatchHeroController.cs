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
            Knight.HeroController.instance.AffectedByGravity(gravityApplies);
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
        if (KnightInSilksong.IsKnight)
        {
            "Leaving Scene Patch".LogInfo();
            Knight.HeroController.instance.LeaveScene();
        }
    }
}

[HarmonyPatch(typeof(HeroController), "LeaveScene", MethodType.Normal)]
public class Patch_HeroController_LeaveScene : GeneralPatch
{
    public static bool Prefix(HeroController __instance, GatePosition? gate)
    {
        return true;
    }
    public static void Postfix(HeroController __instance, GatePosition? gate)
    {
        if (KnightInSilksong.IsKnight)
        {
            "Leave Scene Patch".LogInfo();
            Knight.HeroController.instance.LeaveScene(gate);
        }
    }
}



[HarmonyPatch(typeof(HeroController), "EnterScene", MethodType.Normal)]
public class Patch_HeroController_EnterScene : GeneralPatch
{
    public static bool Prefix(HeroController __instance)
    {
        return true;
    }
    public static void Postfix(HeroController __instance, TransitionPoint enterGate, float delayBeforeEnter)
    {
        if (KnightInSilksong.IsKnight)
        {
            GameManager.instance.StartCoroutine(Knight.HeroController.instance.EnterScene(enterGate, delayBeforeEnter));

        }
    }
}
[HarmonyPatch(typeof(HeroController), "EnterSceneDreamGate", MethodType.Normal)]
public class Patch_HeroController_EnterSceneDreamGate : GeneralPatch
{
    public static bool Prefix(HeroController __instance)
    {
        return true;
    }
    public static void Postfix(HeroController __instance)
    {
        if (KnightInSilksong.IsKnight)
        {
            Knight.HeroController.instance.EnterSceneDreamGate();
            HeroController.instance.gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
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


