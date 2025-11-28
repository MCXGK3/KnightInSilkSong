using KIS;
using KIS.Utils;
using HarmonyLib;
using UnityEngine;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;

[HarmonyPatch(typeof(CheckHeroPerformanceRegionBase), "DoAction")]
public class Patch_CheckHeroPerformanceRegionBase_DoAction : GeneralPatch
{
    public static void Postfix(CheckHeroPerformanceRegionBase __instance)
    {
        if (!KnightInSilksong.IsKnight)
            return;

        GameObject knight = Knight.HeroController.instance.gameObject;
        PlayMakerFSM dnailFsm = knight.GetComponent<PlayMakerFixedUpdate>().playMakerFSMs[6];

        if (dnailFsm.ActiveStateName != "Inactive")
        {
            HeroPerformanceRegion.IsPerforming = true;
            __instance.delay = 0.001f;
        }
        if (dnailFsm.ActiveStateName == "Slash")
            __instance.delay = 0.001f;
        else
        {
            HeroPerformanceRegion.IsPerforming = false;
        }
    }
}
