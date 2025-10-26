using System;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using Mono.Posix;
using KIS;
using UnityEngine;

[HarmonyPatch(typeof(GameObject), "GetComponentByName")]
class PatchGetComponent : GeneralPatch
{
    public static bool Prefix(GameObject __instance, ref Component __result, ref string type)
    {
        switch (type)
        {
            case "HeroController":
                __result = __instance.GetAnyComponent<Knight.HeroController, HeroController>();
                return false;
            case "HeroAudioController":
                __result = __instance.GetAnyComponent<Knight.HeroAudioController, HeroAudioController>();
                return false;
            case "HeroAnimationController":
                __result = __instance.GetAnyComponent<Knight.HeroAnimationController, HeroAnimationController>();
                return false;
            case "HeroBox":
                __result = __instance.GetAnyComponent<Knight.HeroBox, HeroBox>();
                return false;
            case "NailSlash":
                __result = __instance.GetAnyComponent<Knight.NailSlash, NailSlash>();
                return false;
            default:
                return true;
        }
    }

    public static void Postfix(GameObject __instance)
    {

    }
}