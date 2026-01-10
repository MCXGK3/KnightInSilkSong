using KIS;
using KIS.Utils;
using HarmonyLib;
using UnityEngine;
using HutongGames.PlayMaker.Actions;

[HarmonyPatch(typeof(CheckHeroPerformanceRegionV2), "SendEvents")]
public class Patch_CheckHeroPerformanceRegionV2_SendEvents : GeneralPatch
{
    public static bool Prefix(HeroPerformanceRegion.AffectedState affectedState)
    {
        if (!KnightInSilksong.IsKnight)
            return true;

        if (affectedState == HeroPerformanceRegion.AffectedState.ActiveOuter)
            HeroPerformanceRegion.IsPerforming = false;

        return true;
    }
}

