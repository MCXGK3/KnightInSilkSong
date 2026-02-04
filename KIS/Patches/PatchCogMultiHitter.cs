using System.Collections;
using GlobalEnums;
using KIS;

[HarmonyPatch]
public class PatchCogMultiHitter : GeneralPatch
{
    static DefaultDict<CogMultiHitter, Knight.HeroController> cog_hc_dict = new(() => null);
    public static Knight.HeroController damagingKnight;

    [HarmonyPrefix]
    [HarmonyPatch(typeof(CogMultiHitter), nameof(CogMultiHitter.OnTriggerEnter2D), MethodType.Normal)]
    public static bool CogMultiHitter_OnTriggerEnter2D_Prefix(CogMultiHitter __instance, Collider2D other)
    {
        if (KnightInSilksong.IsKnight)
        {
            OnTriggerEnter2DForKnight(__instance, other);
            return false;
        }
        return true;
    }
    [HarmonyPrefix]
    [HarmonyPatch(typeof(CogMultiHitter), nameof(CogMultiHitter.CancelDelay), MethodType.Normal)]
    public static bool CogMultiHitter_CancelDelay_Prefix(CogMultiHitter __instance)
    {
        if (KnightInSilksong.IsKnight)
        {
            if ((bool)cog_hc_dict[__instance])
            {
                cog_hc_dict[__instance].OnHazardRespawn -= __instance.OnHeroHazardRespawn;
            }
        }
        return true;
    }

    public static void OnTriggerEnter2DForKnight(CogMultiHitter __instance, Collider2D other)
    {
        if (Time.timeAsDouble < __instance.canDamageTime || other.gameObject.layer != 20)
        {
            return;
        }

        Knight.HeroController componentInParent = other.GetComponentInParent<Knight.HeroController>();
        if (!componentInParent.isHeroInPosition || componentInParent.playerData.isInvincible)
        {
            return;
        }
        __instance.CancelDelay();
        cog_hc_dict[__instance] = componentInParent;
        componentInParent.OnHazardRespawn += __instance.OnHeroHazardRespawn;
        Vector3 position = ((Component)componentInParent).transform.position;
        float angleToTarget;
        if (__instance.useSelfForAngle)
        {
            angleToTarget = __instance.GetAngleToTarget(position, __instance.transform.position);
        }
        else
        {
            GameObject[] array = GameObject.FindGameObjectsWithTag("Cog Grind Marker");
            Transform transform = null;
            float num = float.PositiveInfinity;
            GameObject[] array2 = array;
            foreach (GameObject gameObject in array2)
            {
                if (!(gameObject == __instance.gameObject))
                {
                    Transform transform2 = gameObject.transform;
                    float sqrMagnitude = (position - transform2.position).sqrMagnitude;
                    if (!(sqrMagnitude > num))
                    {
                        num = sqrMagnitude;
                        transform = transform2;
                    }
                }
            }

            angleToTarget = __instance.GetAngleToTarget(position, transform ? transform.position : __instance.transform.position);
        }

        EventRegister.SendEvent(EventRegisterEvents.CogDamage, __instance.gameObject);
        __instance.hitEffectPrefab.Spawn(position);
        EventRegister.SendEvent(EventRegisterEvents.HeroDamaged);
        StaticVariableList.SetValue("Wound Sender Override", __instance.gameObject);
        FSMUtility.SendEventToGameObject(componentInParent.gameObject, "WOUND START");
        componentInParent.CancelAttack();
        componentInParent.CancelBounce();
        __instance.multiHitRumble.DoShake(__instance, shouldFreeze: false);
        if ((bool)__instance.jitter)
        {
            __instance.jitter.StartJitter();
        }

        if ((bool)__instance.cogAnimator)
        {
            __instance.cogAnimator.FreezePosition = true;
        }

        __instance.heroGrindEffect.SetPosition2D(position);
        __instance.heroGrindEffect.SetRotation2D(angleToTarget);
        __instance.heroGrindEffect.gameObject.SetActive(value: true);
        __instance.heroDamageAudio.SpawnAndPlayOneShot(position);
        __instance.delayRoutine = __instance.StartCoroutine(DelayEndForKnight(__instance, componentInParent));
    }
    public static IEnumerator DelayEndForKnight(CogMultiHitter __instance, Knight.HeroController hc)
    {
        yield return new WaitForSeconds(0.35f);
        __instance.multiHitRumble.CancelShake();
        if ((bool)__instance.jitter)
        {
            __instance.jitter.StopJitter();
        }

        DamageKnightDirectly(__instance, hc);
        yield return new WaitForSeconds(0.2f);
        __instance.heroGrindEffect.gameObject.SetActive(value: false);
    }
    public static void DamageKnightDirectly(CogMultiHitter __instance, Knight.HeroController hc)
    {
        hc.playerData.isInvincible = false;
        hc.cState.focusing = false;
        hc.TakeDamage(__instance.gameObject, (!(__instance.transform.position.x > ((Component)hc).transform.position.x)) ? CollisionSide.left : CollisionSide.right, 1, (int)HazardType.SPIKES);
    }
}