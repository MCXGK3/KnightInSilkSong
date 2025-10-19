using GenericVariableExtension;
using HarmonyLib;
using HutongGames.PlayMaker.Actions;
using TeamCherry.SharedUtils;
using KIS;
using UnityEngine;

[HarmonyPatch(typeof(PlayerData), "SetBool")]
class Patch_PlayerData_SetBool : GeneralPatch
{
    static void Postfix(string boolName, bool value)
    {
        if (KnightInSilksong.IsKnight)
        {
            Knight.PlayerData.instance.SetBool(boolName, value);
        }
    }
}
[HarmonyPatch(typeof(PlayerData), "SetInt")]
class Patch_PlayerData_SetInt : GeneralPatch
{
    static void Postfix(string intName, int value)
    {
        if (KnightInSilksong.IsKnight)
        {
            Knight.PlayerData.instance.SetInt(intName, value);
        }
    }
}
[HarmonyPatch(typeof(PlayerData), "IncrementInt")]
class Patch_PlayerData_IncrementInt : GeneralPatch
{
    static void Postfix(string intName)
    {
        if (KnightInSilksong.IsKnight)
        {
            Knight.PlayerData.instance.IncrementInt(intName);
        }
    }
}
[HarmonyPatch(typeof(PlayerData), "IntAdd")]
class Patch_PlayerData_IntAdd : GeneralPatch
{
    static void Postfix(string intName, int amount)
    {
        if (KnightInSilksong.IsKnight)
        {
            Knight.PlayerData.instance.IntAdd(intName, amount);
        }
    }
}
[HarmonyPatch(typeof(PlayerData), "SetFloat")]
class Patch_PlayerData_SetFloat : GeneralPatch
{
    static void Postfix(string floatName, float value)
    {
        if (KnightInSilksong.IsKnight)
        {
            Knight.PlayerData.instance.SetFloat(floatName, value);
        }
    }
}
[HarmonyPatch(typeof(PlayerData), "DecrementInt")]
class Patch_PlayerData_DecrementInt : GeneralPatch
{
    static void Postfix(string intName)
    {
        if (KnightInSilksong.IsKnight)
        {
            Knight.PlayerData.instance.DecrementInt(intName);
        }
    }
}
[HarmonyPatch(typeof(PlayerData), "SetString")]
class Patch_PlayerData_SetString : GeneralPatch
{
    static void Postfix(string stringName, string value)
    {
        if (KnightInSilksong.IsKnight)
        {
            Knight.PlayerData.instance.SetString(stringName, value);
        }
    }
}
[HarmonyPatch(typeof(PlayerData), "SetVector3")]
class Patch_PlayerData_SetVector3 : GeneralPatch
{
    static void Postfix(string vectorName, Vector3 value)
    {
        if (KnightInSilksong.IsKnight)
        {
            Knight.PlayerData.instance.SetVector3(vectorName, value);
        }
    }
}
[HarmonyPatch(typeof(PlayerData), "SetBenchRespawn", new Type[] { typeof(RespawnMarker), typeof(string), typeof(int) })]
public class Patch_PlayerData_SetBenchRespawn : GeneralPatch
{
    public static bool Prefix(PlayerData __instance, RespawnMarker spawnMarker, string sceneName, int spawnType)
    {
        return true;
    }
    public static void Postfix(PlayerData __instance, RespawnMarker spawnMarker, string sceneName, int spawnType)
    {
        if (KnightInSilksong.IsKnight)
        {
            Knight.PlayerData.instance.respawnMarkerName = spawnMarker.name;
            Knight.PlayerData.instance.respawnScene = sceneName;
            Knight.PlayerData.instance.respawnType = spawnType;
        }
    }
}
[HarmonyPatch(typeof(PlayerData), "SetBenchRespawn", new Type[] { typeof(string), typeof(string), typeof(bool) })]
public class Patch_PlayerData_SetBenchRespawn2 : GeneralPatch
{
    public static bool Prefix(PlayerData __instance, string spawnMarker, string sceneName, bool facingRight)
    {
        return true;
    }
    public static void Postfix(PlayerData __instance, string spawnMarker, string sceneName, bool facingRight)
    {
        if (KnightInSilksong.IsKnight)
        {
            Knight.PlayerData.instance.respawnMarkerName = spawnMarker;
            Knight.PlayerData.instance.respawnScene = sceneName;
        }
    }
}
[HarmonyPatch(typeof(PlayerData), "SetBenchRespawn", new Type[] { typeof(string), typeof(string), typeof(int), typeof(bool) })]
public class Patch_PlayerData_SetBenchRespawn3 : GeneralPatch
{
    public static bool Prefix(PlayerData __instance, string spawnMarker, string sceneName, int spawnType, bool facingRight)
    {
        return true;
    }
    public static void Postfix(PlayerData __instance, string spawnMarker, string sceneName, int spawnType, bool facingRight)
    {
        if (KnightInSilksong.IsKnight)
        {
            Knight.PlayerData.instance.respawnMarkerName = spawnMarker;
            Knight.PlayerData.instance.respawnScene = sceneName;
            Knight.PlayerData.instance.respawnType = spawnType;
        }
    }
}



[HarmonyPatch(typeof(PlayerData), "SetHazardRespawn", new Type[] { typeof(HazardRespawnMarker) })]
public class Patch_PlayerData_SetHazardRespawn : GeneralPatch
{
    public static bool Prefix(PlayerData __instance, HazardRespawnMarker location)
    {
        return true;
    }
    public static void Postfix(PlayerData __instance, HazardRespawnMarker location)
    {
        if (KnightInSilksong.IsKnight)
        {
            Knight.PlayerData.instance.SetHazardRespawn(location);
        }
    }
}
[HarmonyPatch(typeof(PlayerData), "SetHazardRespawn", new Type[] { typeof(Vector3), typeof(bool) })]
public class Patch_PlayerData_SetHazardRespawn_2 : GeneralPatch
{
    public static bool Prefix(PlayerData __instance, Vector3 position, bool facingRight)
    {
        return true;
    }
    public static void Postfix(PlayerData __instance, Vector3 position, bool facingRight)
    {
        if (KnightInSilksong.IsKnight)
        {
            Knight.PlayerData.instance.SetHazardRespawn(position, facingRight);
        }
    }
}
[HarmonyPatch(typeof(PlayerData), "EquipCharm", typeof(int))]
public class Patch_PlayerData_EquipCharm : GeneralPatch
{
    public static bool Prefix(PlayerData __instance, int charmNum)
    {
        return true;
    }
    public static void Postfix(PlayerData __instance, int charmNum)
    {
        if (KnightInSilksong.IsKnight)
        {
            Knight.PlayerData.instance.EquipCharm(charmNum);
        }
    }
}
[HarmonyPatch(typeof(PlayerData), "UnequipCharm", typeof(int))]
public class Patch_PlayerData_UnequipCharm : GeneralPatch
{
    public static bool Prefix(PlayerData __instance, int charmNum)
    {
        return true;
    }
    public static void Postfix(PlayerData __instance, int charmNum)
    {
        if (KnightInSilksong.IsKnight)
        {
            Knight.PlayerData.instance.UnequipCharm(charmNum);
        }
    }
}
// [HarmonyPatch(typeof(VariableExtensions), "SetVariable")]
// static class Patch_PlayerData_SetVariable
// {
//     static void Postfix<T>(IIncludeVariableExtensions obj, string fieldName, T value)
//     {

//         if (TestModPlugin.IsKnight)
//         {
//             if (obj == PlayerData.instance)
//             {
//                 Knight.PlayerData.instance.SetVariable(fieldName, value);
//             }
//         }
//     }
// }