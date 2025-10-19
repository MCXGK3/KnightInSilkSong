[HarmonyPatch(typeof(HitTaker), "Hit", new Type[] { typeof(GameObject), typeof(HitInstance), typeof(int) })]
public class Patch_HitTaker_Hit : GeneralPatch
{
    public static bool Prefix(GameObject targetGameObject, HitInstance damageInstance, int recursionDepth = 3)
    {
        return true;
    }
    public static void Postfix(GameObject targetGameObject, ref HitInstance damageInstance, int recursionDepth = 3)
    {
        ("Hit " + targetGameObject.name + " " + damageInstance.DamageDealt).LogInfo();
    }
}