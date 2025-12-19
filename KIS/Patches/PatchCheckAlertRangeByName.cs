using KIS;
using KIS.Utils;
using HarmonyLib;
using UnityEngine;

[HarmonyPatch(typeof(CheckAlertRangeByName), "Apply")]
public class Patch_CheckAlertRangeByName_Apply : GeneralPatch
{
    public static bool Prefix(CheckAlertRangeByName __instance)
    {
        if (KnightInSilksong.IsKnight)
            HeroController.instance.transform.position = Knight.HeroController.instance.transform.position;

        if (!__instance.sendEvent.IsNone && __instance.source.IsHeroInRange())
        {
            Console.WriteLine("Positive result");
        }
        else if (!__instance.sendEvent.IsNone && !__instance.source.IsHeroInRange())
        {
            Console.WriteLine("Hero not in range");
        }
        else
        {
            Console.WriteLine("No send event");
        }

        return true;
    }

    public static void Postfix(CheckAlertRangeByName __instance)
    {
    }
}

