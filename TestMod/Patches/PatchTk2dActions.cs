using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using KIS;

[HarmonyPatch(typeof(Tk2dPlayAnimation), "DoPlayAnimation", MethodType.Normal)]
public class Patch_Tk2dPlayAnimation_DoPlayAnimation : GeneralPatch
{
    static List<string> KnightAnims = new List<string> {
       "Sit"
    };
    static Dictionary<string, string> hornet_to_knight_anime = new()
    {
        {"Sit","Sit"},
        {"Taunt Back Up","Challenge Start" },
        {"Taunt Straight Back Q","Challenge Start" },
        {"Taunt Back","Challenge Start" },
        {"Taunt Straight Back","Challenge Start"},
        {"Sit Fall Asleep","Sit Fall Asleep"},
        {"Sit Idle","Sit" },
        {"Get Off","Get Off" }
    };
    public static bool Prefix(Tk2dPlayAnimation __instance)
    {
        return true;
    }
    public static void Postfix(Tk2dPlayAnimation __instance)
    {
        if (__instance._sprite != null && KnightInSilksong.IsKnight &&
         ((HeroController.instance != null && __instance._sprite.gameObject == HeroController.instance.gameObject) ||
                                      (__instance._sprite.gameObject == Knight.HeroController.instance.gameObject)))
        {

            if (hornet_to_knight_anime.ContainsKey(__instance.clipName.Value))
            {
                Knight.HeroController.instance.GetComponent<tk2dSpriteAnimator>().Play(hornet_to_knight_anime[__instance.clipName.Value]);

            }

        }
    }
}