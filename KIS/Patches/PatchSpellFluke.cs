using KIS;

[HarmonyPatch(typeof(SpellFluke), "OnEnable", MethodType.Normal)]
public class Patch_SpellFluke_OnEnable : GeneralPatch
{
    public static bool Prefix(SpellFluke __instance)
    {
        return true;
    }
    public static void Postfix(SpellFluke __instance)
    {
        if (KnightInSilksong.IsKnight)
        {
            if (Knight.PlayerData.instance.GetBool("equippedCharm_19"))
            {
                float num = UnityEngine.Random.Range(0.9f, 1.2f);
                __instance.transform.localScale = new Vector3(num, num, 0f);
                __instance.damage = 5;
            }
        }
    }
}