using KIS;
using KIS.Utils;
using HarmonyLib;
using UnityEngine;

[HarmonyPatch(typeof(BounceBalloon), "RaiseMovement")]
public class Patch_BounceBallon_RaiseMovement : GeneralPatch
{
    public static bool Prefix(BounceBalloon __instance, ref Rigidbody2D body)
    {
        if (KnightInSilksong.IsKnight)
        {
            body = Knight.HeroController.instance.GetComponent<Rigidbody2D>();
        }
        return true;
    }

    public static void Postfix(BounceBalloon __instance)
    {
    }
}
