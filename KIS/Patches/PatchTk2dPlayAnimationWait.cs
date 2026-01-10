using HutongGames.PlayMaker.Actions;
using KIS;

[HarmonyPatch(typeof(Tk2dPlayAnimationWait), "OnEnter", MethodType.Normal)]
public class Patch_Tk2dPlayAnimationWait_OnEnter : GeneralPatch
{
    public static void Postfix(Tk2dPlayAnimationWait __instance)
    {
        if (KnightInSilksong.IsKnight)
        {
            if (__instance.sprite.gameObject == Knight.HeroController.instance.gameObject)
            {
                if (__instance.sprite.GetClipByName(__instance.ClipName.Value) == null)
                {
                    __instance.Fsm.Event(__instance.AnimationCompleteEvent);
                    __instance.Finish();
                }
            }
        }
    }
}
