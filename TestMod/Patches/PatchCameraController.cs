[HarmonyPatch(typeof(CameraController), "DoPositionToHero", MethodType.Normal)]
public class Patch_CameraController_DoPositionToHero : GeneralPatch
{
    public static bool Prefix(CameraController __instance, bool forceDirect)
    {
        return true;
    }
    public static void Postfix(CameraController __instance, bool forceDirect)
    {
    }
}
[HarmonyPatch(typeof(CameraController), "LockToArea", MethodType.Normal)]
public class Patch_CameraController_LockToArea : GeneralPatch
{
    public static bool Prefix(CameraController __instance, CameraLockArea lockArea)
    {
        return true;
    }
    public static void Postfix(CameraController __instance, CameraLockArea lockArea)
    {
    }
}

[HarmonyPatch(typeof(CameraController), "ReleaseLock", MethodType.Normal)]
public class Patch_CameraController_ReleaseLock : GeneralPatch
{
    public static bool Prefix(CameraController __instance, CameraLockArea lockarea)
    {
        return true;
    }
    public static void Postfix(CameraController __instance, CameraLockArea lockarea)
    {
    }
}