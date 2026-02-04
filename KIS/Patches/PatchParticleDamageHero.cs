using KIS;

[HarmonyPatch(typeof(ParticleDamageHero), nameof(ParticleDamageHero.Start), MethodType.Normal)]
public class Patch_ParticleDamageHero_Start : GeneralPatch
{
    public static bool Prefix(ParticleDamageHero __instance)
    {
        return true;
    }
    public static void Postfix(ParticleDamageHero __instance)
    {
        if (KnightInSilksong.IsKnight)
        {
            ParticleSystem.TriggerModule trigger = __instance.system.trigger;
            Knight.HeroBox heroBox = Knight.HeroController.instance.gameObject
                .FindGameObjectInChildren("HeroBox").GetComponent<Knight.HeroBox>();
            trigger.AddCollider(heroBox.GetComponent<Collider2D>());
        }
    }
}
[HarmonyPatch(typeof(ParticleDamageHero), nameof(ParticleDamageHero.OnParticleTrigger), MethodType.Normal)]
public class Patch_ParticleDamageHero_OnParticleTrigger : GeneralPatch
{
    public static bool Prefix(ParticleDamageHero __instance)
    {
        if (KnightInSilksong.IsKnight)
        {
            OnParticleTriggerForKnight(__instance);
            return false;
        }
        return true;
    }
    public static void OnParticleTriggerForKnight(ParticleDamageHero __instance)
    {
        if (__instance.system.GetSafeTriggerParticlesSize(ParticleSystemTriggerEventType.Enter) > 0)
        {
            var heroBox = Knight.HeroController.instance.gameObject
                .FindGameObjectInChildren("HeroBox").GetComponent<Knight.HeroBox>();
            if (!Knight.HeroBox.inactive)
            {
                heroBox.CheckForDamageHero(__instance.gameObject);
            }
        }
    }

    public static void Postfix(ParticleDamageHero __instance)
    {
    }
}
