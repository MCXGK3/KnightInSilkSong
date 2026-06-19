using HutongGames.PlayMaker.Actions;
using KIS;

[HarmonyPatch(typeof(tk2dPlayAnimAfterPreviousComplete), nameof(tk2dPlayAnimAfterPreviousComplete.OnEnter), MethodType.Normal)]
public class Patch_tk2dPlayAnimAfterPreviousComplete_OnEnter : GeneralPatch
{
    public static bool Prefix(tk2dPlayAnimAfterPreviousComplete __instance)
    {
        if (KnightInSilksong.IsKnight)
        {
            if (__instance.animator == null || __instance.animator.gameObject == null)
            {
                return true;
            }
            if (__instance.animator.gameObject != Knight.HeroController.instance.gameObject)
            {
                return true;
            }
            if (!__instance.animator.Playing || __instance.animator.CurrentClip?.wrapMode != tk2dSpriteAnimationClip.WrapMode.Once)
            {
                __instance.OnAnimationCompleted(__instance.animator, null);
                __instance.Finish();
                return false;
            }
        }
        return true;
    }
    public static void Postfix(tk2dPlayAnimAfterPreviousComplete __instance)
    {
    }
}