[HarmonyPatch(typeof(TransitionPoint), "PrepareEntry", MethodType.Normal)]
public class Patch_TransitionPoint_PrepareEntry : GeneralPatch
{
    public static void Postfix(TransitionPoint __instance)
    {
        "PrepareEntry Patch".LogInfo();
        Time.time.LogInfo();
    }
}
