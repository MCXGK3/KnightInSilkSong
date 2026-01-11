using KIS;
using HutongGames.PlayMaker.Actions;

[HarmonyPatch(typeof(CheckHeroPerformanceRegionBase), "DoAction")]
public class Patch_CheckHeroPerformanceRegionBase_DoAction : GeneralPatch
{
    public static void Postfix(CheckHeroPerformanceRegionBase __instance)
    {
        if (!KnightInSilksong.IsKnight)
            return;

        GameObject knight = Knight.HeroController.instance.gameObject;
        PlayMakerFSM dnailFsm = knight.LocateMyFSM("Dream Nail");

        if (dnailFsm.ActiveStateName == "Slash")
        {
            HeroPerformanceRegion.IsPerforming = true;
            __instance.delay = 0.001f;
        }
    }
}

