using KIS;

[HarmonyPatch(typeof(InventoryItemToolManager), "IsAvailable", MethodType.Normal)]
public class Patch_InventoryItemToolManager_IsAvailable : GeneralPatch
{
    public static bool Prefix(InventoryItemToolManager __instance, ref bool __result)
    {
        if (KnightInSilksong.IsKnight)
        {
            if (CollectableItemManager.IsInHiddenMode())
            {
                __result = false;
            }
            else
            {
                __result = true;
            }
            return false;
        }
        return true;
    }
    public static void Postfix(InventoryItemToolManager __instance)
    {
    }
}