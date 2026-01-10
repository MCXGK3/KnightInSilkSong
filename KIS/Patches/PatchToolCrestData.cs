using KIS;

[HarmonyPatch(typeof(SerializableNamedList<ToolCrestsData.Data, ToolCrestsData.NamedData>), nameof(ToolCrestsData.SetData), MethodType.Normal)]
public class Patch_ToolCrestsData_SetData : GeneralPatch
{

    public static void Postfix(SerializableNamedList<ToolCrestsData.Data, ToolCrestsData.NamedData> __instance, string itemName, ToolCrestsData.Data data)
    {
        SyncManager.Instance.H2KSyncData(SyncManager.FromUnlockedSlots.CheckKey);
    }
}