using System;
using System.IO;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using Mono.Posix;
using KIS;
using UnityEngine;

[HarmonyPatch(typeof(CallMethodProper), "PreCache")]
class PatchCallMethodProper : GeneralPatch
{
    public static bool Prefix(CallMethodProper __instance)
    {
        // Log(__instance.fsm.name);
        // Log(__instance.fsm.GameObject.name);
        return true;
    }

    public static void Postfix(CallMethodProper __instance)
    {

    }
    static void Log(object msg)
    {
        KnightInSilksong.logger.LogInfo(msg);
    }
    static void Orig_PreCache(CallMethodProper __instance)
    {
        GameObject ownerDefaultTarget = __instance.Fsm.GetOwnerDefaultTarget(__instance.gameObject);
        if (ownerDefaultTarget == null)
        {
            Log("ownerDefaultTarget is null ");
            return;
        }

        __instance.component = ownerDefaultTarget.GetComponent(__instance.behaviour.Value) as MonoBehaviour;
        if (__instance.component == null)
        {
            Log("component is null ");
            return;
        }

        __instance.cachedType = __instance.component.GetType();
        try
        {
            __instance.cachedMethodInfo = __instance.cachedType.GetMethod(__instance.methodName.Value);
        }
        catch (AmbiguousMatchException)
        {

            Type[] types = __instance.parameters.Select((FsmVar fsmVar) => fsmVar.RealType).ToArray();
            __instance.cachedMethodInfo = __instance.cachedType.GetMethod(__instance.methodName.Value, types);
        }

        if (__instance.cachedMethodInfo == null)
        {
            __instance.errorString = __instance.errorString + "Method Name is invalid: " + __instance.methodName.Value + "\n";
        }
        else
        {
            __instance.cachedParameterInfo = __instance.cachedMethodInfo.GetParameters();
        }
    }
}

[HarmonyPatch(typeof(CallMethodProper), "DoMethodCall")]
class PatchDoMethodCall : GeneralPatch
{
    public static bool Prefix(CallMethodProper __instance)
    {
        // Log(__instance.fsm.name);
        // Log(__instance.fsm.GameObject.name);
        // return true;
        try
        {
            Modified_DoMethodCall(__instance);
        }
        catch (Exception e)
        {
            Log(__instance.fsm.GameObject.name + " " + __instance.fsm.name + " " + __instance.State.name + " " + __instance.behaviour.value + __instance.methodName.value);
            Log(e);
        }
        return false;
    }

    public static void Postfix(CallMethodProper __instance)
    {

    }
    static void Log(object msg)
    {
        KnightInSilksong.logger.LogInfo(msg);
    }
    public static void WarpToDreamGate(GameManager gm)
    {
        gm.entryGateName = "dreamGate";
        gm.targetScene = Knight.PlayerData.instance.GetString("dreamGateScene");
        gm.entryDelay = 0f;
        gm.cameraCtrl.FreezeInPlace(false);
        gm.BeginSceneTransition(new GameManager.SceneLoadInfo
        {
            AlwaysUnloadUnusedAssets = true,
            EntryGateName = "dreamGate",
            PreventCameraFadeOut = true,
            SceneName = Knight.PlayerData.instance.GetString("dreamGateScene"),
            Visualization = GameManager.SceneLoadVisualizations.ThreadMemory
        });
    }

    private static void Modified_DoMethodCall(CallMethodProper __instance)
    {

        if (string.IsNullOrEmpty(__instance.behaviour.Value) || string.IsNullOrEmpty(__instance.methodName.Value))
        {
            __instance.Finish();
            return;
        }
        if (__instance.behaviour.value == "CharmIconList" && __instance.methodName.value == "GetSprite")
        {
            if (CharmIconList.Instance != null)
            {
                __instance.parameters[0].UpdateValue();
                __instance.storeResult.SetValue(CharmIconList.Instance.GetSprite((int)__instance.parameters[0].intValue));
            }
            __instance.Finish();
            return;
        }
        if (KnightInSilksong.IsKnight)
        {
            if (__instance.behaviour.value == "HeroController")
            {
                if (__instance.methodName.value == "TakeQuickDamage")
                {
                    bool temp_inv = Knight.PlayerData.instance.isInvincible;
                    Knight.PlayerData.instance.isInvincible = false;
                    Knight.HeroController.instance.TakeDamage(null, GlobalEnums.CollisionSide.other, __instance.parameters[0].intValue, (int)KnightInSilksong.HazardType_NORESPOND);
                    Knight.PlayerData.instance.isInvincible = temp_inv;
                    __instance.Finish();
                    return;
                }
                else if (__instance.methodName.value == "TakeQuickDamageSimple")
                {
                    bool temp_inv = Knight.PlayerData.instance.isInvincible;
                    Knight.PlayerData.instance.isInvincible = false;
                    Knight.HeroController.instance.TakeDamage(null, GlobalEnums.CollisionSide.other, __instance.parameters[0].intValue, (int)KnightInSilksong.HazardType_NORESPOND);
                    Knight.PlayerData.instance.isInvincible = temp_inv;
                    __instance.Finish();
                    return;
                }
                else if (__instance.methodName.value == "WillDoBellBindHit")
                {
                    __instance.storeResult.SetValue(false);
                    __instance.Finish();
                    return;
                }
                else if (__instance.methodName.value == "ActivateVoidAcid")
                {
                    __instance.Finish();
                    return;
                }
                else if (__instance.methodName.value == "CanTakeDamage")
                {
                    __instance.storeResult.SetValue(Traverse.Create(Knight.HeroController.instance).Method("CanTakeDamage").GetValue<bool>());
                    __instance.Finish();
                    return;
                }
            }
            if (__instance.behaviour.value == "GameManager")
            {
                if (__instance.methodName.value == "WarpToDreamGate")
                {
                    WarpToDreamGate(GameManager.instance);
                    __instance.Finish();
                    return;
                }
            }
        }

        GameObject ownerDefaultTarget = __instance.Fsm.GetOwnerDefaultTarget(__instance.gameObject);
        if (ownerDefaultTarget == null)
        {
            return;
        }

        __instance.component = ownerDefaultTarget.GetComponent(__instance.behaviour.Value) as MonoBehaviour;

        if (__instance.component == null)
        {
            __instance.LogWarning("CallMethodProper: " + ownerDefaultTarget.name + " missing behaviour: " + __instance.behaviour.Value);
            return;
        }

        if (__instance.cachedMethodInfo == null || !__instance.cachedMethodInfo.Name.Equals(__instance.methodName.Value))
        {
            __instance.errorString = string.Empty;
            if (!__instance.DoCache())
            {
                Debug.LogError(__instance.errorString, __instance.Owner);
                __instance.Finish();
                return;
            }
        }

        object value = null;
        if (__instance.cachedParameterInfo.Length == 0)
        {
            try
            {
                value = __instance.cachedMethodInfo.Invoke(__instance.component, null);
            }
            catch (TargetException)//Catch Another Exception
            {
                value = __instance.cachedMethodInfo.Invoke(__instance.component.GetAnotherComponent(), null);
            }
            catch (Exception e)
            {
                Debug.LogError("CallMethodProper error on " + __instance.Fsm.OwnerName + " -> " + e, __instance.Owner);
            }
        }
        else
        {
            for (int i = 0; i < __instance.parameters.Length; i++)
            {
                FsmVar fsmVar = __instance.parameters[i];
                fsmVar.UpdateValue();
                __instance.parametersArray[i] = fsmVar.GetValue();
            }

            try
            {
                value = __instance.cachedMethodInfo.Invoke(__instance.component, __instance.parametersArray);
            }
            catch (TargetParameterCountException)
            {
                ParameterInfo[] array = __instance.cachedMethodInfo.GetParameters();
                Debug.LogErrorFormat(__instance.Owner, "Count did not match. Required: {0}, Was: {1}, Method: {2}", array.Length, __instance.parametersArray.Length, __instance.cachedMethodInfo.Name);
            }
            catch (TargetException)
            {
                value = __instance.cachedMethodInfo.Invoke(__instance.component.GetAnotherComponent(), __instance.parametersArray);
            }
            catch (Exception ex2)
            {
                Debug.LogError("CallMethodProper error on " + __instance.Fsm.OwnerName + " -> " + ex2, __instance.Owner);
            }
        }

        if (__instance.storeResult.Type != VariableType.Unknown)
        {
            __instance.storeResult.SetValue(value);
        }
    }



}