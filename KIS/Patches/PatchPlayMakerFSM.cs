using HutongGames.PlayMaker.Actions;
using KIS;
using KIS.Utils;
using UnityEngine.SceneManagement;

[HarmonyPatch(typeof(PlayMakerFSM), "Start")]
public class Patch_PlayMakerFSM_Start : GeneralPatch
{
    public static bool Prefix(PlayMakerFSM __instance)
    {
        if (KnightInSilksong.IsKnight)
        {
            string fsmName = __instance.FsmName.ToLower();
            string goName = __instance.gameObject.name.ToLower();
            string sceneName = SceneManager.GetActiveScene().name.ToLower();

            // if (sceneName == "bone_05" && goName == "boss scene" && fsmName == "battle end")
            //     bell_beast_skip_silkheart(__instance);
            if (sceneName == "belltown_shrine" && goName == "spinner boss" && fsmName == "control")
                widow_skip_focus(__instance);
            if (/*sceneName == "bone_05" &&*/ goName == "silk heart" && fsmName == "control")
            {
                FixGetSilkHeart(__instance);
            }
            if (goName.StartsWith("silk spool ui") && fsmName == "silk spool ui")
            {
                FixGetSilkSpool(__instance);
            }

        }
        if (__instance.gameObject.name.StartsWith("Hollow Shade Death") && __instance.FsmName == "Shade Control")
        {
            PreventGetGeoFromShade(__instance);
        }

        return true;
    }

    private static void PreventGetGeoFromShade(PlayMakerFSM fsm)
    {
        fsm.GetAction<CallMethodProper>("Death Start", 3).enabled = false;
        fsm.GetAction<CallMethodProper>("Give Geo", 3).enabled = false;
        fsm.InsertCustomAction("Destroy", () => HeroController.instance.CocoonBroken(), 0);
    }

    private static void FixGetSilkHeart(PlayMakerFSM fsm)
    {
        fsm.AddCustomAction("Regen Last Silk", (fsm) =>
        {
            fsm.SendEvent("REGENERATED SILK CHUNK");
        });
    }

    private static void FixGetSilkSpool(PlayMakerFSM fsm)
    {
        if (fsm.fsm.GetFsmGameObject("Hero").Value == null)
        {
            ("Get Error SilkSpool at " + SceneManager.GetActiveScene().name).LogWarning();
        }
        else
        {
            fsm.fsm.GetFsmGameObject("Hero").value = KISHelper.GetCurrentHero();
        }
    }

    private static void bell_beast_skip_silkheart(PlayMakerFSM fsm)
    {
        fsm.ChangeTransition("Idle", "BATTLE END", "End Pause");
    }

    private static void widow_skip_focus(PlayMakerFSM fsm)
    {
        fsm.ChangeTransition("Hornet Dash", "FINISHED", "Final Bind Burst");
    }
}

