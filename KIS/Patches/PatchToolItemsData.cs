using KIS;


[HarmonyPatch(typeof(SerializableNamedList<ToolItemsData.Data, ToolItemsData.NamedData>), nameof(ToolItemsData.SetData), MethodType.Normal)]
public class Patch_ToolItemsData_SetData : GeneralPatch
{
    public static bool Prefix(SerializableNamedList<ToolItemsData.Data, ToolItemsData.NamedData> __instance, string itemName, ToolItemsData.Data data)
    {

        return true;
    }
    public static void Postfix(SerializableNamedList<ToolItemsData.Data, ToolItemsData.NamedData> __instance, string itemName, ToolItemsData.Data data)
    {
        SyncManager.Instance.H2KSyncData(itemName);
    }
}