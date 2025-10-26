using GlobalEnums;
using HutongGames.PlayMaker.Actions;
using KIS;

[HarmonyPatch(typeof(DamageHeroDirectly), "OnEnter", MethodType.Normal)]
public class Patch_DamageHeroDirectly_OnEnter : GeneralPatch
{
    public static bool Prefix(DamageHeroDirectly __instance)
    {
        if (KnightInSilksong.IsKnight)
        {
            GameObject ownerDefaultTarget = __instance.Fsm.GetOwnerDefaultTarget(__instance.damager);
            if (ownerDefaultTarget == null)
            {
                __instance.Finish();
                return false;
            }
            Knight.PlayerData.instance.isInvincible = false;
            HazardType hazardType = HazardType.ENEMY;
            if (__instance.spikeHazard)
            {
                hazardType = HazardType.SPIKES;
            }
            else if (__instance.sinkHazard)
            {
                hazardType = HazardType.SINK;
            }
            if (ownerDefaultTarget.transform.position.x > HeroController.instance.gameObject.transform.position.x)
            {
                Knight.HeroController.instance.TakeDamage(ownerDefaultTarget.gameObject, CollisionSide.right, __instance.damageAmount.Value, (int)hazardType);
            }
            else
            {
                Knight.HeroController.instance.TakeDamage(ownerDefaultTarget.gameObject, CollisionSide.left, __instance.damageAmount.Value, (int)hazardType);
            }
            __instance.Finish();
            return false;
        }
        return true;
    }
    public static void Postfix(DamageHeroDirectly __instance)
    {
    }
}