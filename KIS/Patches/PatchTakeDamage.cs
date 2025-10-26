using HutongGames.PlayMaker.Actions;
using KIS;

[HarmonyPatch(typeof(TakeDamage), "OnEnter", MethodType.Normal)]
public class Patch_TakeDamage_OnEnter : GeneralPatch
{
    public static bool Prefix(TakeDamage __instance)
    {
        if (KnightInSilksong.IsKnight && __instance.fsm.GetFsmBool("FromKnight") != null)
        {
            HitTaker.Hit(__instance.Target.Value, new HitInstance
            {
                Source = __instance.Owner,
                AttackType = (AttackTypes)__instance.AttackType.Value,
                CircleDirection = __instance.CircleDirection.Value,
                DamageDealt = __instance.DamageDealt.Value,
                Direction = __instance.Direction.Value,
                IgnoreInvulnerable = __instance.IgnoreInvulnerable.Value,
                MagnitudeMultiplier = __instance.MagnitudeMultiplier.Value,
                MoveAngle = __instance.MoveAngle.Value,
                MoveDirection = __instance.MoveDirection.Value,
                Multiplier = (__instance.Multiplier.IsNone ? 1f : __instance.Multiplier.Value),
                SpecialType = (SpecialTypes)(__instance.SpecialType.Value | KnightInSilksong.KnightDamage),
                IsFirstHit = true,
                IsHeroDamage = true
            });
            __instance.Finish();
            return false;
        }
        return true;
    }
    public static void Postfix(TakeDamage __instance)
    {
    }
}