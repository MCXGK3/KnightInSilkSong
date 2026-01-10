using KIS;
using KIS.Utils;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;
using BepInEx;

[HarmonyPatch(typeof(PlayMakerFSM), "Start")]
public class Patch_PlayMakerFSM_Start : GeneralPatch
{
    public static bool Prefix(PlayMakerFSM __instance)
    {
        string fsmName = __instance.FsmName.ToLower();
        string goName = __instance.gameObject.name.ToLower();
        string sceneName = SceneManager.GetActiveScene().name.ToLower();

        if (sceneName == "bone_05" && goName == "boss scene" && fsmName == "battle end")
            bell_beast_skip_silkheart(__instance);
        if (sceneName == "belltown_shrine" && goName == "spinner boss" && fsmName == "control")
            widow_skip_focus(__instance);

        return true;
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

