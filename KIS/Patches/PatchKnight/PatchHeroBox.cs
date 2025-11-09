using GlobalEnums;
using KIS;

[HarmonyPatch(typeof(Knight.HeroBox), "CheckForDamage", MethodType.Normal)]
public class Patch_Knight_HeroBox_CheckForDamage : GeneralPatch
{
    public static DamagePropertyFlags flags;
    public static bool Prefix(Knight.HeroBox __instance, Collider2D otherCollider)
    {
        return true;
    }
    public static void Postfix(Knight.HeroBox __instance, Collider2D otherCollider)
    {
        if (KnightInSilksong.IsKnight)
        {
            DamageHero component = otherCollider.gameObject.GetComponent<DamageHero>();
            if (component != null && (!Knight.HeroController.instance.cState.shadowDashing /*|| !component.shadowDashHazard*/))
            {
                flags = component.damagePropertyFlags;
            }
        }
    }
}
[HarmonyPatch(typeof(Knight.HeroBox), "ApplyBufferedHit", MethodType.Normal)]
public class Patch_Knight_HeroBox_ApplyBufferedHit : GeneralPatch
{
    public static bool Prefix(Knight.HeroBox __instance)
    {
        if (KnightInSilksong.IsKnight)
        {
            var field = Traverse.Create(__instance).Field("damageDealt");
            if (field.GetValue<int>() > 0)
            {
                if ((Patch_Knight_HeroBox_CheckForDamage.flags & DamagePropertyFlags.Flame) != DamagePropertyFlags.None)
                {
                    field.SetValue(2);
                }
            }
        }
        return true;
    }
    public static void Postfix(Knight.HeroBox __instance)
    {
    }
}