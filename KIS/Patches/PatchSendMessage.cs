using HutongGames.PlayMaker.Actions;
using KIS;

[HarmonyPatch(typeof(SendMessage), "DoSendMessage", MethodType.Normal)]
public class Patch_SendMessage_DoSendMessage : GeneralPatch
{
    static List<string> herocontroller_methods = new()
    {
        "RegainControl",
        "RelinquishControl",
        "StartAnimationControl",
        "StartAnimationControlToIdle",
        "StartAnimationControlToIdleForcePlay" 
        // {"AffectedByGravity",HeroController.instance.AffectedByGravity() },
        // {"EnableWallJump",HeroController.instance.EnableWallJump },
        // {"DisableWallJump",HeroController.instance.DisableWallJump },
        // {"EnableSuperDash",HeroController.instance.EnableSuperDash },
        // {"DisableSuperDash",HeroController.instance.DisableSuperDash }

    
    };
    public static bool Prefix(SendMessage __instance)
    {
        if (KnightInSilksong.IsKnight)
        {
            if (__instance.functionCall.FunctionName.IsAny([.. herocontroller_methods]))
            {
                Traverse.Create(HeroController.instance).Method(__instance.functionCall.FunctionName).GetValue();
                __instance.Finish();
                return false;
            }
        }

        return true;
    }
    public static void Postfix(SendMessage __instance)
    {
    }
}