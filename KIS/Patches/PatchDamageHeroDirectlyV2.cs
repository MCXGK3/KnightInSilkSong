using GlobalEnums;
using HutongGames.PlayMaker.Actions;
using KIS;
using static HutongGames.PlayMaker.Actions.DamageHeroDirectlyV2;

[HarmonyPatch(typeof(DamageHeroDirectlyV2), "OnEnter", MethodType.Normal)]
public class Patch_DamageHeroDirectlyV2_OnEnter : GeneralPatch
{
    public static bool Prefix(DamageHeroDirectlyV2 __instance)
    {
        if (KnightInSilksong.IsKnight)
        {
            ModifiedOnEnter(__instance);
            return false;
        }
        return true;
    }
    public static void Postfix(DamageHeroDirectlyV2 __instance)
    {
    }
    public static void ModifiedOnEnter(DamageHeroDirectlyV2 __instance)
    {
        GameObject ownerDefaultTarget = __instance.Fsm.GetOwnerDefaultTarget(__instance.damager);
        if (ownerDefaultTarget == null)
        {
            return;
        }

        Knight.HeroController instance = Knight.HeroController.instance;
        PlayerData.instance.isInvincible = false;
        HazardType hazardType = (HazardType)(object)__instance.hazardType.Value;
        if (__instance.overrideDirection.IsNone)
        {
            switch ((DamageHeroDirectlyV2.DamageDirection)(object)__instance.damageDirection.Value)
            {
                case DamageDirection.FromDamageSource:
                    if (__instance.invertDirection.Value)
                    {
                        if (ownerDefaultTarget.transform.position.x < instance.gameObject.transform.position.x)
                        {
                            instance.TakeDamage(ownerDefaultTarget.gameObject, CollisionSide.right, __instance.damageAmount.Value, __instance.hazardType.ToInt());
                        }
                        else
                        {
                            instance.TakeDamage(ownerDefaultTarget.gameObject, CollisionSide.left, __instance.damageAmount.Value, __instance.hazardType.ToInt());
                        }
                    }
                    else if (ownerDefaultTarget.transform.position.x > instance.gameObject.transform.position.x)
                    {
                        instance.TakeDamage(ownerDefaultTarget.gameObject, CollisionSide.right, __instance.damageAmount.Value, __instance.hazardType.ToInt());
                    }
                    else
                    {
                        instance.TakeDamage(ownerDefaultTarget.gameObject, CollisionSide.left, __instance.damageAmount.Value, __instance.hazardType.ToInt());
                    }

                    break;
                case DamageDirection.FromHeroFacingDirection:
                    if (__instance.invertDirection.Value)
                    {
                        if (instance.transform.localScale.x > 0f)
                        {
                            instance.TakeDamage(ownerDefaultTarget.gameObject, CollisionSide.right, __instance.damageAmount.Value, __instance.hazardType.ToInt());
                        }
                        else
                        {
                            instance.TakeDamage(ownerDefaultTarget.gameObject, CollisionSide.left, __instance.damageAmount.Value, __instance.hazardType.ToInt());
                        }
                    }
                    else if (instance.transform.localScale.x < 0f)
                    {
                        instance.TakeDamage(ownerDefaultTarget.gameObject, CollisionSide.right, __instance.damageAmount.Value, __instance.hazardType.ToInt());
                    }
                    else
                    {
                        instance.TakeDamage(ownerDefaultTarget.gameObject, CollisionSide.left, __instance.damageAmount.Value, __instance.hazardType.ToInt());
                    }

                    break;
            }
        }
        else
        {
            instance.TakeDamage(ownerDefaultTarget.gameObject, (CollisionSide)(object)__instance.overrideDirection.Value, __instance.damageAmount.Value, __instance.hazardType.ToInt());
        }

        __instance.Finish();
    }
}