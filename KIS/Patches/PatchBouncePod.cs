using KIS;
using KIS.Utils;
using HarmonyLib;
using UnityEngine;

[HarmonyPatch(typeof(BouncePod), "Hit")]
public class Patch_BouncePod_Hit : GeneralPatch
{
    private static HitInstance damageInstance;
    static bool Prefix(HitInstance damageInstance)
    {
        Patch_BouncePod_Hit.damageInstance = damageInstance;
        return true;
    }

    static void Postfix(IHitResponder.HitResponse __result)
    {
        Console.WriteLine("SOMEthing thing");
        if (KnightInSilksong.IsKnight && __result == IHitResponder.Response.GenericHit)
        {
            HitInstance.HitDirection hitDirection = damageInstance.GetHitDirection(HitInstance.TargetType.BouncePod);
            if (hitDirection == HitInstance.HitDirection.Down)
            {
                Knight.HeroController.instance.ShroomBounce();
            }
        }
    }
}
