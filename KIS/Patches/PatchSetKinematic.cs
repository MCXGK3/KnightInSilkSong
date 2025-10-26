using HutongGames.PlayMaker.Actions;
using KIS;

[HarmonyPatch(typeof(SetIsKinematic2d), "DoSetIsKinematic", MethodType.Normal)]
public class Patch_SetIsKinematic2d_DoSetIsKinematic //: GeneralPatch
{
    public static bool Prefix(SetIsKinematic2d __instance)
    {
        return true;
    }
    public static void Postfix(SetIsKinematic2d __instance)
    {
        if (KnightInSilksong.IsKnight)
        {
            if (__instance.Fsm.GetOwnerDefaultTarget(__instance.gameObject) == HeroController.instance.gameObject)
            {
                Rigidbody2D rb2d = Knight.HeroController.instance.GetComponent<Rigidbody2D>();
                {
                    rb2d.isKinematic = __instance.isKinematic.Value;
                }
            }
        }
    }
}