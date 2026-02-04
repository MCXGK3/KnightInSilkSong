using System.Collections;
using KIS;

[HarmonyPatch]
public class Patch_HeroPlatformStick : GeneralPatch
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(HeroPlatformStick), nameof(HeroPlatformStick.OnCollisionEnter2D), MethodType.Normal)]
    public static bool OnCollisionEnter2D_Prefix(HeroPlatformStick __instance, Collision2D collision)
    {
        if (KnightInSilksong.IsKnight)
        {
            OnCollisionEnter2DForKnight(__instance, collision);
            return false;
        }
        return true;
    }
    static void OnCollisionEnter2DForKnight(HeroPlatformStick __instance, Collision2D collision)
    {
        if ((bool)__instance.insideTracker)
        {
            return;
        }

        GameObject gameObject = collision.gameObject;
        if (gameObject.layer != 9)
        {
            return;
        }

        var component = gameObject.GetComponent<Knight.HeroController>();
        if ((bool)component && !(collision.GetSafeContact().Normal.y >= 0f))
        {
            if (HeroPlatformStick._waitRoutine != null)
            {
                __instance.StopCoroutine(HeroPlatformStick._waitRoutine);
                HeroPlatformStick._waitRoutine = null;
            }

            if (component.cState.onGround)
            {
                __instance.wasInside = true;
                __instance.Refresh();
            }
            else
            {
                HeroPlatformStick._waitRoutine = __instance.StartCoroutine(WaitForGrounded(__instance, component));
            }
        }
    }
    public static IEnumerator WaitForGrounded(HeroPlatformStick __instance, Knight.HeroController heroController)
    {
        while (!heroController.cState.onGround)
        {
            yield return null;
        }

        __instance.wasInside = true;
        __instance.Refresh();
    }
    [HarmonyPrefix]
    [HarmonyPatch(typeof(HeroPlatformStick), nameof(HeroPlatformStick.OnCollisionExit2D), MethodType.Normal)]
    public static bool OnCollisionExit2D_Prefix(HeroPlatformStick __instance, Collision2D collision)
    {
        if (KnightInSilksong.IsKnight)
        {
            OnCollisionExit2DForKnight(__instance, collision);
            return false;
        }
        return true;
    }
    static void OnCollisionExit2DForKnight(HeroPlatformStick __instance, Collision2D collision)
    {
        if ((bool)__instance.insideTracker)
        {
            return;
        }

        GameObject gameObject = collision.gameObject;
        if (gameObject.layer == 9 && (bool)gameObject.GetComponent<Knight.HeroController>())
        {
            if (HeroPlatformStick._waitRoutine != null)
            {
                __instance.StopCoroutine(HeroPlatformStick._waitRoutine);
                HeroPlatformStick._waitRoutine = null;
            }

            __instance.wasInside = false;
            __instance.Refresh();
        }
    }
    [HarmonyPostfix]
    [HarmonyPatch(typeof(HeroPlatformStick), nameof(HeroPlatformStick.DoParent))]
    static void OnDoParent(HeroPlatformStick __instance, HeroController heroController)
    {
        // if use `Interpolate`, the hero's running(and dashing) speed will be very very slow.
        Knight.HeroController.instance.rb2d.interpolation = RigidbodyInterpolation2D.None;
    }
    [HarmonyPostfix]
    [HarmonyPatch(typeof(HeroPlatformStick), nameof(HeroPlatformStick.DoDeparent))]
    static void OnDoDeParent(HeroPlatformStick __instance, HeroController heroController)
    {
        Knight.HeroController.instance.rb2d.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

}
