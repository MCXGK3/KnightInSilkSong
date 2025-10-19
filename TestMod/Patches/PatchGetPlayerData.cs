using GenericVariableExtension;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using KIS;
using KIS.Utils;

[HarmonyPatch(typeof(GetPlayerDataBool), "OnEnter", MethodType.Normal)]
public class Patch_GetPlayerDataBool_OnEnter : GeneralPatch
{
    public static bool Prefix(GetPlayerDataBool __instance)
    {
        if (KnightInSilksong.IsKnight && __instance.fsm.GetVariable<FsmBool>("FromKnight") != null)
        {
            if (__instance.boolName.value == "EncounteredLostLace")
            {
                (__instance.fsm.name + " " + __instance.State.name + " GetPlayerDataBool").LogInfo();
            }
            __instance.storeValue.Value = Knight.PlayerData.instance.GetBool(__instance.boolName.Value);
            __instance.Finish();
            return false;
        }
        return true;
    }
    public static void Postfix(GetPlayerDataBool __instance)
    {
    }
}
[HarmonyPatch(typeof(GetPlayerDataFloat), "OnEnter", MethodType.Normal)]
public class Patch_GetPlayerDataFloat_OnEnter : GeneralPatch
{
    public static bool Prefix(GetPlayerDataFloat __instance)
    {
        if (KnightInSilksong.IsKnight && __instance.fsm.GetVariable<FsmBool>("FromKnight") != null)
        {
            __instance.storeValue.Value = Knight.PlayerData.instance.GetFloat(__instance.floatName.Value);
            __instance.Finish();
            return false;
        }
        return true;
    }
    public static void Postfix(GetPlayerDataFloat __instance)
    {
    }
}

[HarmonyPatch(typeof(GetPlayerDataInt), "OnEnter", MethodType.Normal)]
public class Patch_GetPlayerDataInt_OnEnter : GeneralPatch
{
    public static bool Prefix(GetPlayerDataInt __instance)
    {
        if (KnightInSilksong.IsKnight && __instance.fsm.GetVariable<FsmBool>("FromKnight") != null)
        {
            __instance.storeValue.Value = Knight.PlayerData.instance.GetInt(__instance.intName.Value);
            __instance.Finish();
            return false;
        }
        return true;
    }
    public static void Postfix(GetPlayerDataInt __instance)
    {
    }
}
[HarmonyPatch(typeof(GetPlayerDataString), "OnEnter", MethodType.Normal)]
public class Patch_GetPlayerDataString_OnEnter : GeneralPatch
{
    public static bool Prefix(GetPlayerDataString __instance)
    {
        if (KnightInSilksong.IsKnight && __instance.fsm.GetVariable<FsmBool>("FromKnight") != null)
        {
            __instance.storeValue.Value = Knight.PlayerData.instance.GetString(__instance.stringName.Value);
            __instance.Finish();
            return false;
        }
        return true;
    }
    public static void Postfix(GetPlayerDataString __instance)
    {
    }
}
[HarmonyPatch(typeof(GetPlayerDataVector3), "OnEnter", MethodType.Normal)]
public class Patch_GetPlayerDataVector3_OnEnter : GeneralPatch
{
    public static bool Prefix(GetPlayerDataVector3 __instance)
    {
        if (KnightInSilksong.IsKnight && __instance.fsm.GetVariable<FsmBool>("FromKnight") != null)
        {
            __instance.storeValue.Value = Knight.PlayerData.instance.GetVector3(__instance.vector3Name.Value);
            __instance.Finish();
            return false;
        }
        return true;
    }
    public static void Postfix(GetPlayerDataVector3 __instance)
    {
    }
}

[HarmonyPatch(typeof(PlayerDataBoolTest), "OnEnter", MethodType.Normal)]
public class Patch_PlayerDataBoolTest_OnEnter : GeneralPatch
{
    public static bool Prefix(PlayerDataBoolTest __instance)
    {
        if (KnightInSilksong.IsKnight && __instance.fsm.GetVariable<FsmBool>("FromKnight") != null)
        {
            if (__instance.boolName.value == "EncounteredLostLace")
            {
                (__instance.fsm.name + " " + __instance.State.name + " PlayerDataBoolTest").LogInfo();
            }
            bool boolCheck = Knight.PlayerData.instance.GetBool(__instance.boolName.Value);
            __instance.fsm.Event(boolCheck ? __instance.isTrue : __instance.isFalse);
            __instance.Finish();
            return false;
        }
        return true;
    }
    public static void Postfix(PlayerDataBoolTest __instance)
    {
    }
}
[HarmonyPatch(typeof(PlayerDataBoolAllTrue), "DoAllTrue", MethodType.Normal)]
public class Patch_PlayerDataBoolAllTrue_DoAllTrue : GeneralPatch
{
    public static bool Prefix(PlayerDataBoolAllTrue __instance)
    {
        if (KnightInSilksong.IsKnight && __instance.fsm.GetVariable<FsmBool>("FromKnight") != null)
        {
            bool flag = true;
            for (int i = 0; i < __instance.stringVariables.Length; i++)
            {
                if (!Knight.PlayerData.instance.GetBool(__instance.stringVariables[i].Value))
                {
                    flag = false;
                    break;
                }
            }

            if (flag)
            {
                __instance.Fsm.Event(__instance.trueEvent);
            }
            else
            {
                __instance.Fsm.Event(__instance.falseEvent);
            }

            __instance.storeResult.Value = flag;
            return false;
        }
        return true;
    }
    public static void Postfix(PlayerDataBoolAllTrue __instance)
    {
    }
}

[HarmonyPatch(typeof(PlayerDataBoolTrueAndFalse), "OnEnter", MethodType.Normal)]
public class Patch_PlayerDataBoolTrueAndFalse_OnEnter : GeneralPatch
{
    public static bool Prefix(PlayerDataBoolTrueAndFalse __instance)
    {
        if (KnightInSilksong.IsKnight && __instance.fsm.GetVariable<FsmBool>("FromKnight") != null)
        {
            if (Knight.PlayerData.instance.GetBool(__instance.trueBool.Value) && !Knight.PlayerData.instance.GetBool(__instance.falseBool.Value))
            {
                __instance.Fsm.Event(__instance.isTrue);
            }
            else
            {
                __instance.Fsm.Event(__instance.isFalse);
            }
            return false;
        }
        return true;
    }
    public static void Postfix(PlayerDataBoolTrueAndFalse __instance)
    {
    }
}