using HutongGames.PlayMaker.Actions;
using KIS;

[HarmonyPatch(typeof(Tk2dWatchAnimationEvents), "OnUpdate", MethodType.Normal)]
public class Patch_Tk2dWatchAnimationEvents_OnUpdate : GeneralPatch
{
    public static bool Prefix(Tk2dWatchAnimationEvents __instance)
    {
        if (KnightInSilksong.IsKnight)
        {
            if (__instance._sprite == Knight.HeroController.instance.gameObject.GetComponent<tk2dSpriteAnimator>())
            {
                if (!__instance._sprite.Playing)
                {
                    if (__instance.animationTriggerEvent != null)
                    {
                        __instance.Fsm.Event(__instance.animationTriggerEvent);
                    }
                }
            }
        }
        return true;
    }
}
