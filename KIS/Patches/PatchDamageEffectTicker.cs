using KIS;

[HarmonyPatch(typeof(DamageEffectTicker), "DoFlashOnEnemy", MethodType.Normal)]
public class Patch_DamageEffectTicker_DoFlashOnEnemy : GeneralPatch
{
    public static bool Prefix(DamageEffectTicker __instance, GameObject enemy)
    {
        return true;
    }
    public static void Postfix(DamageEffectTicker __instance, GameObject enemy)
    {
        if (KnightInSilksong.IsKnight)
        {
            if (__instance.enemySpriteFlash == DamageEffectTicker.SpriteFlashMethods.None)
            {
                string name = __instance.name;
                var flash = enemy.GetComponent<SpriteFlash>();
                if (flash != null)
                {
                    if (name.Contains("Spore"))
                    {
                        flash.flashSporeQuick();
                    }
                    else if (name.Contains("Dung"))
                    {
                        flash.flashDungQuick();
                    }
                }
            }
        }
    }
}