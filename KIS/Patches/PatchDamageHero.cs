using GlobalEnums;
using HutongGames.PlayMaker;
using KIS;
using KIS.Utils;

[HarmonyPatch(typeof(DamageHero), "TryClashTinkCollider", MethodType.Normal)]
public class Patch_DamageHero_TryClashTinkCollider : GeneralPatch
{
    public static bool Prefix(DamageHero __instance, Collider2D collision)
    {
        if (KnightInSilksong.IsKnight)
        {
            string text = collision.gameObject.tag;
            Transform obj = collision.transform;
            Vector3 position = obj.position;
            Transform parent = obj.parent;
            if (!parent)
            {
                return true;
            }
            if (text == "Nail Attack")
            {
                if (!__instance.canClashTink || __instance.nailClashRoutine != null || __instance.preventClashTink || collision.gameObject.layer != 16)
                {
                    ("Cant Clash " + collision.gameObject.name + " " + __instance.name).LogInfo();
                    (!__instance.canClashTink + " " + (__instance.nailClashRoutine != null) + " " + __instance.preventClashTink).LogInfo();
                    return false;
                }
                ("Clash yes" + collision.gameObject.name + " " + __instance.name).LogInfo();
                Knight.HeroController hc = Knight.HeroController.instance;
                if (text == "Nail Attack" && !__instance.noClashFreeze && hc.parryInvulnTimer < Mathf.Epsilon)
                {
                    ("try freeze " + collision.gameObject.name).LogInfo();
                    GameManager.instance.FreezeMoment(FreezeMomentTypes.NailClashEffect);
                }
                float direction = 0f;
                var fsm = collision.gameObject.transform.parent.gameObject.LocateMyFSM("damages_enemy");
                if (fsm != null)
                {
                    direction = fsm.GetVariable<FsmFloat>("direction").Value;
                }
                __instance.nailClashRoutine = __instance.StartCoroutine(__instance.NailClash(direction, text, position));
                return false;
            }
        }
        return true;
    }
    public static void Postfix(DamageHero __instance, Collider2D collision)
    {
    }
}
[HarmonyPatch(typeof(DamageHero), "OnDamaged", MethodType.Normal)]
public class Patch_DamageHero_OnDamaged : GeneralPatch
{
    public static bool Prefix(DamageHero __instance)
    {
        if (KnightInSilksong.IsKnight)
        {
            return false;
        }
        return true;
    }
    public static void Postfix(DamageHero __instance)
    {
    }
}