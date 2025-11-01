using static HitInstance;

[HarmonyPatch(typeof(HitInstance), "GetActualDirection", MethodType.Normal)]
public class Patch_HitInstance_GetActualDirection : GeneralPatch
{
    public static bool Prefix(ref HitInstance __instance, Transform target, TargetType targetType)
    {
        return true;
    }
    public static void Postfix(ref HitInstance __instance, Transform target, TargetType targetType)
    {

    }
}