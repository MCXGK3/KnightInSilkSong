using KIS;
using KIS.Utils;
using HarmonyLib;
using UnityEngine;

[HarmonyPatch(typeof(BouncePod), "DoBounceOff")]
public class Patch_BouncePod_DoBounceOff : GeneralPatch
{
    static bool Prefix()
    {
        if (KnightInSilksong.IsKnight)
        {
            Knight.HeroController.instance.ShroomBounce();
            return false;
        }
        return true;
    }
}
