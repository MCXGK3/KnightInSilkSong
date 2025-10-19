using HarmonyLib;
using HutongGames.PlayMaker.Actions;
using KIS;
using UnityEngine;

[HarmonyPatch(typeof(SetPosition), "DoSetPosition")]
internal class Patch_SetPosition_DoSetPosition : GeneralPatch
{
    static void Postfix(SetPosition __instance)
    {
        if (KnightInSilksong.IsKnight)
        {
            if (__instance.cachedTransform == HeroController.instance.transform)
            {
                Vector3 vector = ((!__instance.vector.IsNone) ? __instance.vector.Value : ((__instance.space == Space.World) ? __instance.cachedTransform.position : __instance.cachedTransform.localPosition));
                if (!__instance.x.IsNone)
                {
                    vector.x = __instance.x.Value;
                }

                if (!__instance.y.IsNone)
                {
                    vector.y = __instance.y.Value;
                }

                if (!__instance.z.IsNone)
                {
                    vector.z = __instance.z.Value;
                }

                if (__instance.space == Space.World)
                {
                    Knight.HeroController.instance.transform.position = vector;
                }
                else
                {
                    Knight.HeroController.instance.transform.localPosition = vector;
                }
            }
        }
    }

}
[HarmonyPatch(typeof(SetPosition2D), "DoSetPosition")]
internal class Patch_SetPosition2D_DoSetPosition : GeneralPatch
{
    static void Postfix(SetPosition2D __instance)
    {
        if (KnightInSilksong.IsKnight)
        {
            GameObject ownerdefault = null;
            try
            {
                ownerdefault = __instance.Fsm.GetOwnerDefaultTarget(__instance.GameObject);
            }
            catch (NullReferenceException)
            {
                string msg = "";
                msg += __instance.fsm.name + " ";
                msg += __instance.fsmState.name + " ";
                msg += __instance.fsm.GameObject.name + " ";
                if (__instance != null)
                {
                    if (__instance.GameObject != null)
                    {
                        msg += __instance.GameObject + " ";
                    }
                }
                else
                {
                    msg += "null";
                }
                msg.LogInfo();
            }
            if (ownerdefault == HeroController.instance.gameObject)
            {
                Knight.HeroController.instance.transform.position = HeroController.instance.transform.position;
            }


        }
    }

}