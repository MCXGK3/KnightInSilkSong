using KIS;

[HarmonyPatch(typeof(CameraLockArea), "OnInsideStateChanged", MethodType.Normal)]
public class Patch_CameraLockArea_OnInsideStateChanged : GeneralPatch
{
    public static bool Prefix(CameraLockArea __instance, bool isInside)
    {

        return true;
    }
    public static void Postfix(CameraLockArea __instance, bool isInside)
    {
    }
}
[HarmonyPatch(typeof(CameraLockArea), "IsInApplicableGameState", MethodType.Normal)]
public class Patch_CameraLockArea_IsInApplicableGameState : GeneralPatch
{
    public static bool Prefix(ref bool __result)
    {
        if (KnightInSilksong.IsKnight)
        {
            if (GameManager.UnsafeInstance != null)
            {
                if (GameManager.UnsafeInstance.GameState == GlobalEnums.GameState.EXITING_LEVEL)
                {
                    __result = true;
                    return false;
                }
            }
        }
        return true;
    }
    public static void Postfix()
    {
    }
}