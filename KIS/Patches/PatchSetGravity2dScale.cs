using HutongGames.PlayMaker.Actions;
using KIS;

[HarmonyPatch(typeof(SetGravity2dScale), nameof(SetGravity2dScale.DoSetGravityScale), MethodType.Normal)]
public class Patch_SetGravity2dScale_DoSetGravityScale : GeneralPatch
{
    public static bool Prefix(SetGravity2dScale __instance)
    {
        if (KnightInSilksong.IsKnight)
        {
            GameObject ownerDefaultTarget = __instance.Fsm.GetOwnerDefaultTarget(__instance.gameObject);
            if (ownerDefaultTarget == Knight.HeroController.instance.gameObject && __instance.gravityScale.Value <= Mathf.Epsilon)
            {
                Knight.HeroController.instance.AffectedByGravity(false);
                return false;
            }
        }
        return true;
    }
    public static void Postfix(SetGravity2dScale __instance)
    {
    }
}