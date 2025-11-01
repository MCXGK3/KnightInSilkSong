using HutongGames.PlayMaker.Actions;
using KIS;

[HarmonyPatch(typeof(InstaDeath), "OnEnter", MethodType.Normal)]
public class Patch_InstaDeath_OnEnter : GeneralPatch
{
    public static bool Prefix(InstaDeath __instance)
    {
        if (KnightInSilksong.IsKnight)
        {
            ModifyOnEnter(__instance);

            return false;
        }
        return true;
    }
    public static void Postfix(InstaDeath __instance)
    {
    }
    public static void ModifyOnEnter(InstaDeath __instance)
    {
        GameObject safe = __instance.target.GetSafe(__instance);
        if (safe != null)
        {
            HealthManager component = safe.GetComponent<HealthManager>();
            if (component != null)
            {
                if (!component.isDead)
                {
                    float value = (__instance.direction.IsNone ? DirectionUtils.GetAngle(component.GetAttackDirection()) : __instance.direction.Value);
                    component.Die(value, AttackTypes.Generic, NailElements.None, null, ignoreEvasion: false, 1f, overrideSpecialDeath: true);
                }
            }
            else
            {
                if (safe.GetComponent<EnemyDeathEffects>() != null)
                {
                    safe.GetComponent<EnemyDeathEffects>().ReceiveDeathEvent(DirectionUtils.GetAngle(1), AttackTypes.Generic, 0f);
                }
            }
        }

        __instance.Finish();
    }
}