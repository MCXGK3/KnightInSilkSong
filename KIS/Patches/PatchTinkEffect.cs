using GlobalEnums;
using KIS;
using GlobalSettings;
using MonoMod.Utils;

[HarmonyPatch(typeof(TinkEffect), "Hit", MethodType.Normal)]
public class Patch_TinkEffect_Hit : GeneralPatch
{
    public static bool Prefix(TinkEffect __instance, ref HitInstance damageInstance, ref IHitResponder.HitResponse __result)
    {
        if (KnightInSilksong.IsKnight)
        {
            if (((int)damageInstance.SpecialType & KnightInSilksong.KnightDamage) != 0)
            {
                GameObject gameObject = damageInstance.Source;
                if (gameObject.layer != 17)
                {
                    __result = IHitResponder.Response.None;
                }
                bool flag = gameObject.CompareTag("Nail Attack");
                Vector2 tinkPosition;
                __result = TryDoTinkReactionNoDamagerWithoutDamageEnemies(ref damageInstance, __instance, damageInstance.Source, true, true, flag, out tinkPosition) ? IHitResponder.Response.GenericHit : IHitResponder.Response.None;
                return false;
            }
        }

        return true;
    }
    public static void Postfix(TinkEffect __instance)
    {
    }

    private static float GetActualHitDirectionWithoutDE(TinkEffect __instance, ref HitInstance hitInstance)
    {


        if (!hitInstance.CircleDirection)
        {
            return hitInstance.Direction;
        }

        Vector2 vector = (Vector2)__instance.transform.position - (Vector2)hitInstance.Source.transform.position;
        return Mathf.Atan2(vector.y, vector.x) * 57.29578f;
    }

    public static bool TryDoTinkReactionNoDamagerWithoutDamageEnemies(ref HitInstance damageInstance, TinkEffect __instance, GameObject obj, bool doCamShake, bool doSound, bool isNailAttack, out Vector2 tinkPosition)
    {
        bool flag = true;
        bool flag2 = Time.timeAsDouble >= TinkEffect._nextTinkTime;
        NailAttackBase component = obj.GetComponent<NailAttackBase>();
        bool flag3 = isNailAttack && (bool)component && !component.CanHitSpikes;
        HeroController instance = HeroController.instance;
        Vector3 position = obj.transform.position;
        tinkPosition = position;
        Vector3 vector = (isNailAttack ? instance.transform.position : position);
        if (__instance.useOwnYPosition)
        {
            position.y = (vector.y = __instance.transform.position.y);
        }

        // DamageEnemies component2 = obj.GetComponent<DamageEnemies>();
        // bool flag4 = component2 != null;
        // if (!flag4)
        // {
        //     component2 = obj.transform.parent.gameObject.GetComponent<DamageEnemies>();
        //     flag4 = component2 != null;
        //     if (!flag4)
        //     {
        //         return false;
        //     }
        // }
        bool flag4 = true;

        float actualHitDirection;
        if (isNailAttack)
        {
            if ((bool)__instance.heroDamager && __instance.heroDamager.hazardType == HazardType.SPIKES && flag3)
            {
                instance.TakeDamage(__instance.heroDamager.gameObject, CollisionSide.top, __instance.heroDamager.damageDealt, HazardType.SPIKES, __instance.heroDamager.damagePropertyFlags);
                return false;
            }

            actualHitDirection = GetActualHitDirectionWithoutDE(__instance, ref damageInstance);
        }
        else
        {
            actualHitDirection = GetActualHitDirectionWithoutDE(__instance, ref damageInstance);
            ITinkResponder component3 = obj.GetComponent<ITinkResponder>();
            bool flag5 = component3 != null;
            if (flag5)
            {
                if ((__instance.ignoreResponders & component3.ResponderType) != ITinkResponder.TinkFlags.None)
                {
                    return false;
                }

                component3.Tinked();
            }

            if (__instance.onlyReactToNail)
            {
                if (!flag5)
                {
                    return false;
                }

                flag = false;
            }
        }

        int cardinalDirection = DirectionUtils.GetCardinalDirection(actualHitDirection);
        switch (cardinalDirection)
        {
            case 3:
                if (!__instance.directionMask.IsBitSet(3))
                {
                    return false;
                }

                __instance.TryPreventDamage(/*component2*/null, 3);
                break;
            case 1:
                if (!__instance.directionMask.IsBitSet(2))
                {
                    return false;
                }

                __instance.TryPreventDamage(/*component2*/null, 2);
                break;
            case 2:
                if (!__instance.directionMask.IsBitSet(0))
                {
                    return false;
                }

                if (isNailAttack && __instance.checkSlashPosition && obj.transform.position.x < __instance.transform.position.x)
                {
                    return false;
                }

                __instance.TryPreventDamage(/*component2*/null, 0);
                break;
            case 0:
                if (!__instance.directionMask.IsBitSet(1))
                {
                    return false;
                }

                if (isNailAttack && __instance.checkSlashPosition && obj.transform.position.x > __instance.transform.position.x)
                {
                    return false;
                }

                __instance.TryPreventDamage(/*component2*/null, 1);
                break;
        }

        if (flag2 && __instance.RecoilHero && flag4)
        {
            // component2.OnTinkEffectTink();
        }

        bool flag6 = __instance.gameObject.GetComponent<NonBouncer>();
        if (flag4)
        {
            int layer = __instance.gameObject.layer;
            if ((layer == 11 || layer == 17 || layer == 19) && !flag6)
            {
                // component2.OnBounceableTink();
                switch (cardinalDirection)
                {
                    case 3:
                        // component2.OnBounceableTinkDown();
                        break;
                    case 1:
                        // component2.OnBounceableTinkUp();
                        break;
                    case 2:
                        // component2.OnBounceableTinkLeft();
                        break;
                    case 0:
                        // component2.OnBounceableTinkRight();
                        break;
                }
            }
        }

        if (isNailAttack)
        {
            if (flag2)
            {
                TinkEffect._nextTinkTime = Time.timeAsDouble + 0.009999999776482582;
            }

            if (!__instance.gameCam)
            {
                __instance.gameCam = GameCameras.instance;
            }

            if (doCamShake && flag2 && (bool)__instance.gameCam)
            {
                if (__instance.overrideCamShake)
                {
                    __instance.camShakeOverride.DoShake(__instance);
                }
                else
                {
                    __instance.gameCam.cameraShakeFSM.SendEvent("EnemyKillShake");
                }
            }
        }

        Vector3 euler = new Vector3(0f, 0f, 0f);
        bool flag7 = __instance.collider != null;
        if (__instance.useNailPosition && (!flag4))
        {
            flag7 = false;
        }

        Vector2 vector2 = Vector2.zero;
        float num = 0f;
        float num2 = 0f;
        if (flag7)
        {
            Bounds bounds = __instance.collider.bounds;
            vector2 = __instance.transform.TransformPoint(__instance.collider.offset);
            num = bounds.size.x * 0.5f;
            num2 = bounds.size.y * 0.5f;
        }

        Vector3 vector3;
        switch (cardinalDirection)
        {
            case 0:
                if (isNailAttack && flag2 && __instance.RecoilHero)
                {
                    instance.RecoilLeft();
                }

                if (flag)
                {
                    if (__instance.sendDirectionalFSMEvents && (bool)__instance.fsm)
                    {
                        __instance.fsm.SendEvent("TINK RIGHT");
                    }

                    __instance.SendHitInDirection(obj, HitInstance.HitDirection.Right);
                }

                if (flag7)
                {
                    float num5 = Mathf.Max(0f, num2 - 1.5f);
                    position.y = Mathf.Clamp(position.y, vector2.y - num5, vector2.y + num5);
                    vector3 = new Vector3(vector2.x - num, position.y, 0.002f);
                }
                else
                {
                    vector3 = ((!isNailAttack) ? new Vector3(vector.x, vector.y, 0.002f) : new Vector3(vector.x + 2f, vector.y, 0.002f));
                }

                break;
            case 1:
                if (isNailAttack && flag2 && __instance.RecoilHero)
                {
                    instance.RecoilDown();
                }

                if (flag)
                {
                    if (__instance.sendDirectionalFSMEvents && (bool)__instance.fsm)
                    {
                        __instance.fsm.SendEvent("TINK UP");
                    }

                    __instance.SendHitInDirection(obj, HitInstance.HitDirection.Up);
                }

                vector3 = (flag7 ? new Vector3(position.x, Mathf.Max(vector2.y - num2, position.y), 0.002f) : ((!isNailAttack) ? new Vector3(vector.x, vector.y, 0.002f) : new Vector3(vector.x, vector.y + 2f, 0.002f)));
                euler = new Vector3(0f, 0f, 90f);
                break;
            case 2:
                if (isNailAttack && flag2 && __instance.RecoilHero)
                {
                    instance.RecoilRight();
                }

                if (flag)
                {
                    if (__instance.sendDirectionalFSMEvents && (bool)__instance.fsm)
                    {
                        __instance.fsm.SendEvent("TINK LEFT");
                    }

                    __instance.SendHitInDirection(obj, HitInstance.HitDirection.Left);
                }

                if (!flag7)
                {
                    vector3 = ((!isNailAttack) ? new Vector3(vector.x, vector.y, 0.002f) : new Vector3(vector.x - 2f, vector.y, 0.002f));
                }
                else
                {
                    float num4 = Mathf.Max(0f, num2 - 1.5f);
                    position.y = Mathf.Clamp(position.y, vector2.y - num4, vector2.y + num4);
                    vector3 = new Vector3(vector2.x + num, position.y, 0.002f);
                }

                euler = new Vector3(0f, 0f, 180f);
                break;
            default:
                if (flag)
                {
                    if (__instance.sendDirectionalFSMEvents && (bool)__instance.fsm)
                    {
                        __instance.fsm.SendEvent("TINK DOWN");
                        if (isNailAttack)
                        {
                            __instance.fsm.SendEvent(instance.cState.facingRight ? "TINK DOWN R" : "TINK DOWN L");
                        }
                    }

                    __instance.SendHitInDirection(obj, HitInstance.HitDirection.Down);
                }

                if (!flag7)
                {
                    vector3 = ((!isNailAttack) ? new Vector3(vector.x, vector.y, 0.002f) : new Vector3(vector.x, vector.y - 2f, 0.002f));
                }
                else
                {
                    float num3 = position.x;
                    if (num3 < vector2.x - num)
                    {
                        num3 = vector2.x - num;
                    }

                    if (num3 > vector2.x + num)
                    {
                        num3 = vector2.x + num;
                    }

                    vector3 = new Vector3(num3, Mathf.Min(vector2.y + num2, position.y), 0.002f);
                }

                euler = new Vector3(0f, 0f, 270f);
                break;
        }

        GameObject gameObject = (flag3 ? Effects.TinkEffectDullPrefab : __instance.blockEffect);
        if (flag2)
        {
            Quaternion rotation = Quaternion.Euler(euler);
            if ((bool)gameObject)
            {
                AudioSource component4 = gameObject.Spawn(vector3, rotation).GetComponent<AudioSource>();
                if ((bool)component4)
                {
                    component4.pitch = UnityEngine.Random.Range(0.85f, 1.15f);
                    component4.volume = (doSound ? 1f : 0f);
                }
            }
            // __instance.OnSpawnedTink?.Invoke(vector3, rotation);//有问题
        }

        tinkPosition = vector3;
        if (!flag)
        {
            return true;
        }

        if (__instance.sendFSMEvent && (bool)__instance.fsm)
        {
            __instance.fsm.SendEvent(__instance.FSMEvent);
        }

        __instance.OnTinked.Invoke();
        if (flag4 && Knight.PlayerData.instance.equippedCharm_15)
        {
            __instance.OnTinkedHeavy.Invoke();
        }

        switch (cardinalDirection)
        {
            case 3:
                __instance.OnTinkedDown.Invoke();
                break;
            case 1:
                __instance.OnTinkedUp.Invoke();
                break;
            case 2:
                __instance.OnTinkedLeft.Invoke();
                break;
            case 0:
                __instance.OnTinkedRight.Invoke();
                break;
        }

        return true;
    }



}