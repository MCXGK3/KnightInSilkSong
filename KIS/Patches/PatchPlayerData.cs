using HutongGames.PlayMaker.Actions;
using KIS;
using PrepatcherPlugin;
using PDVE = PrepatcherPlugin.PlayerDataVariableEvents;
using PDA = PrepatcherPlugin.PlayerDataAccess;
using Unity.Audio;
internal static class BlackList
{
    public static readonly HashSet<string> HDFields = [.. typeof(PlayerData).GetFields().Select(x => x.Name)];
    public static readonly HashSet<string> KDFields = [.. typeof(Knight.PlayerData).GetFields().Select(x => x.Name)];
    public static readonly HashSet<string> BlackListedFields = [.. HDFields.Except(KDFields)];
    public static readonly HashSet<string> should_handle_fields = [.. KDFields.Except(HDFields)];
    public static bool IsBlackListed(string var_name)
    {
        return BlackListedFields.Contains(var_name);
    }
    public static bool ShouldHandle(string var_name)
    {
        return should_handle_fields.Contains(var_name);
    }
}
internal static class HandlePlayerData
{
    private static Knight.PlayerData kd => Knight.PlayerData.instance;
    public static void Init()
    {
        PDVE.OnGetBool += HandleGetBool;
        PDVE.OnGetInt += HandleGetInt;
        PDVE.OnGetFloat += HandleGetFloat;
        PDVE.OnGetString += HandleGetString;
        PDVE.OnGetVector3 += HandleGetVector3;
    }

    private static Vector3 HandleGetVector3(PlayerData pd, string fieldName, Vector3 current)
    {
        if (BlackList.ShouldHandle(fieldName))
        {
            return kd.GetVector3(fieldName);
        }
        return current;
    }

    private static string HandleGetString(PlayerData pd, string fieldName, string current)
    {
        if (BlackList.ShouldHandle(fieldName))
        {
            return kd.GetString(fieldName);
        }
        return current;
    }

    private static float HandleGetFloat(PlayerData pd, string fieldName, float current)
    {
        if (BlackList.ShouldHandle(fieldName))
        {
            return kd.GetFloat(fieldName);
        }
        return current;
    }

    private static int HandleGetInt(PlayerData pd, string fieldName, int current)
    {
        if (BlackList.ShouldHandle(fieldName))
        {
            return kd.GetInt(fieldName);
        }
        return current;
    }

    private static bool HandleGetBool(PlayerData pd, string fieldName, bool current)
    {
        if (BlackList.ShouldHandle(fieldName))
        {
            return kd.GetBool(fieldName);
        }
        return current;
    }

}

[HarmonyPatch(typeof(PlayerData), "SetBool")]
class Patch_PlayerData_SetBool : GeneralPatch
{
    static void Postfix(string boolName, bool value)
    {
        if (KnightInSilksong.IsKnight)
        {
            try
            {

                if (!SyncManager.Instance.IsWatching(boolName) && !BlackList.IsBlackListed(boolName))
                {
                    Knight.PlayerData.instance.SetBool(boolName, value);
                }

                if (boolName == "atBench")
                {
                    if (value)
                    {
                        PlayMakerFSM.BroadcastEvent("BENCHREST");
                    }
                    else
                    {
                        PlayMakerFSM.BroadcastEvent("BENCHREST END");
                    }
                }
                SyncManager.Instance.H2KSyncData(boolName);
            }
            catch (ArgumentException e)
            {
                ("ArgumentException " + boolName + " " + value).LogError();
            }
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
            SyncManager.Instance.H2KSyncData(intName);
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
            if (!SyncManager.Instance.IsWatching(floatName) && !BlackList.IsBlackListed(floatName))
            {
                Knight.PlayerData.instance.SetFloat(floatName, value);
            }
            SyncManager.Instance.H2KSyncData(floatName);
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
            SyncManager.Instance.H2KSyncData(stringName);
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
            SyncManager.Instance.H2KSyncData(vectorName);
        }
    }
}
[HarmonyPatch(typeof(PlayerData), "SetBenchRespawn", new Type[] { typeof(RespawnMarker), typeof(string), typeof(int) })]
public class Patch_PlayerData_SetBenchRespawn : GeneralPatch
{
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
