using KIS;
using GlobalEnums;

[HarmonyPatch(typeof(Knight.HeroController), "LocateSpawnPoint", MethodType.Normal)]
public class Patch_HeroController_LocateSpawnPoint : GeneralPatch
{
    public static bool Prefix(Knight.HeroController __instance, ref Transform __result)
    {
        if (KnightInSilksong.IsKnight)
        {
            __result = global::HeroController.instance.LocateSpawnPoint();
            return false;
        }
        return true;
    }
}
[HarmonyPatch(typeof(Knight.HeroController), "CharmUpdate", MethodType.Normal)]
public class Patch_HeroController_CharmUpdate : GeneralPatch
{
    public static void Postfix(Knight.HeroController __instance)
    {
        if (KnightInSilksong.IsKnight)
        {
            PlayMakerFSM.BroadcastEvent("CHARM EQUIP CHECK");
            PlayMakerFSM.BroadcastEvent("CHARM INDICATOR CHECK");
            EventRegister.SendEvent("UPDATE BLUE HEALTH");
        }
    }
}
[HarmonyPatch(typeof(Knight.HeroController), "Attack", MethodType.Normal)]
public class Patch_HeroController_Attack : GeneralPatch
{
    public static bool Prefix(Knight.HeroController __instance, AttackDirection attackDir)
    {
        if (KnightInSilksong.IsKnight)
        {
            HeroController.instance.IncrementAttackCounter();
        }
        return true;
    }
}

[HarmonyPatch(typeof(Knight.HeroController), "FinishedEnteringScene", MethodType.Normal)]
public class Patch_Knight_HeroController_FinishedEnteringScene : GeneralPatch
{
    public static void Postfix()
    {
        EventRegister.SendEvent("ENTERING SCENE");
    }
}

[HarmonyPatch(typeof(Knight.HeroController), "TakeDamage", MethodType.Normal)]
public class Patch_Knight_HeroController_TakeDamage : GeneralPatch
{
    public static HazardType last_hazard_type;
    public static bool Prefix(Knight.HeroController __instance, GameObject go, CollisionSide damageSide, int damageAmount, ref int hazardType)
    {
        if (KnightInSilksong.IsKnight)
        {
            last_hazard_type = (HazardType)hazardType;
            if (hazardType == (int)HazardType.LAVA)
            {
                hazardType = (int)HazardType.SPIKES;
            }
            else if (hazardType == KnightInSilksong.HazardType_NORESPOND)
            {

                hazardType = (int)HazardType.LAVA;
            }
            else if (hazardType != 1 && hazardType != 8)
            {
                hazardType = (int)HazardType.SPIKES;

            }

        }
        return true;
    }
}
[HarmonyPatch(typeof(Knight.HeroController), "Die", MethodType.Enumerator)]
public class Patch_Knight_HeroController_Die : GeneralPatch
{
    public static bool Prefix(Knight.HeroController __instance)
    {
        "Try Dead".LogInfo();
        HeroController.instance.cState.dead = true;
        GameManager.instance.StartCoroutine(HeroController.instance.Die(false, false));
        "Try Dead".LogInfo();
        return true;
    }
}
[HarmonyPatch(typeof(Knight.HeroController), "CanTakeDamage", MethodType.Normal)]
public class Patch_Knight_HeroController_CanTakeDamage : GeneralPatch
{
    public static bool Prefix(Knight.HeroController __instance, ref bool __result)
    {
        if (__instance.damageMode == DamageMode.HAZARD_ONLY || __instance.cState.shadowDashing || __instance.parryInvulnTimer > 0)
        {
            __result = false;
            return false;
        }
        return true;
    }
}
[HarmonyPatch(typeof(Knight.HeroController), "HazardRespawn", MethodType.Normal)]
public class Patch_Knight_HeroController_HazardRespawn : GeneralPatch
{
    public static bool Prefix(Knight.HeroController __instance)
    {
        __instance.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        return true;
    }
}

[HarmonyPatch]
public class Give_One_WallJump : GeneralPatch
{
    public static bool used_walljump = false;
    [HarmonyPostfix]
    [HarmonyPatch(typeof(Knight.HeroController), "DoWallJump", MethodType.Normal)]
    public static void Knight_HeroController_DoWallJump_PostFix(Knight.HeroController __instance)
    {
        if (KnightInSilksong.IsKnight)
        {
            used_walljump = true;
        }
    }
    [HarmonyPostfix]
    [HarmonyPatch(typeof(Knight.HeroController), "BackOnGround", MethodType.Normal)]
    public static void Postfix(Knight.HeroController __instance)
    {
        if (KnightInSilksong.IsKnight)
        {
            used_walljump = false;
        }
    }


}
[HarmonyPatch(typeof(Knight.HeroController), "CanInfiniteAirJump", MethodType.Normal)]
public class Patch_Knight_HeroController_CanInfiniteAirJump : GeneralPatch
{
    public static void Postfix(Knight.HeroController __instance, ref bool __result)
    {

        if (__result)
        {
            var animator = Knight.HeroController.instance.GetComponent<tk2dSpriteAnimator>();
            animator.Stop();
            animator.state.LogInfo();
        }

    }
}
[HarmonyPatch]
public class Patch_Knight_HeroController_DieFromHazard : GeneralPatch
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Knight.HeroController), nameof(Knight.HeroController.DieFromHazard), MethodType.Normal)]
    public static bool Prefix(Knight.HeroController __instance, ref HazardType hazardType, float angle)
    {
        if (Patch_Knight_HeroController_TakeDamage.last_hazard_type == HazardType.LAVA)
        {
            hazardType = HazardType.LAVA;
        }
        return true;
    }
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Knight.HeroController), nameof(Knight.HeroController.DieFromHazard), MethodType.Normal)]
    public static void Postfix(Knight.HeroController __instance, HazardType hazardType, float angle)
    {
        if (hazardType == HazardType.LAVA)
        {
            GameObject obj4 = HeroController.instance.lavaDeathPrefab.Spawn();
            obj4.transform.position = Knight.HeroController.instance.transform.position;
            obj4.transform.localScale = Knight.HeroController.instance.transform.localScale;
            DeliveryQuestItem.BreakAll();
        }
    }
}
[HarmonyPatch(typeof(Knight.HeroController), nameof(Knight.HeroController.FaceLeft), MethodType.Normal)]
public class Patch_Knight_HeroController_FaceLeft : GeneralPatch
{
    public static bool Prefix(Knight.HeroController __instance)
    {
        return true;
    }
    public static void Postfix(Knight.HeroController __instance)
    {
        Vector3 scale = __instance.transform.localScale;
        scale.x *= KnightInSilksong.knight_scaleX.Value;
        scale.y = KnightInSilksong.knight_scaleY.Value;
        __instance.transform.SetScale2D(scale);
    }
}
[HarmonyPatch(typeof(Knight.HeroController), nameof(Knight.HeroController.FaceRight), MethodType.Normal)]
public class Patch_Knight_HeroController_FaceRight : GeneralPatch
{
    public static bool Prefix(Knight.HeroController __instance)
    {
        return true;
    }
    public static void Postfix(Knight.HeroController __instance)
    {
        Vector3 scale = __instance.transform.localScale;
        scale.x *= KnightInSilksong.knight_scaleX.Value;
        scale.y = KnightInSilksong.knight_scaleY.Value;
        __instance.transform.SetScale2D(scale);
    }
}
[HarmonyPatch(typeof(Knight.HeroController), nameof(Knight.HeroController.Update10), MethodType.Normal)]
public class Patch_Knight_HeroController_Update10 : GeneralPatch
{
    public static bool Prefix(Knight.HeroController __instance)
    {
        return true;
    }
    public static void Postfix(Knight.HeroController __instance)
    {
        Vector3 scale = __instance.transform.localScale;
        scale.x *= KnightInSilksong.knight_scaleX.Value;
        scale.y = KnightInSilksong.knight_scaleY.Value;
        __instance.transform.SetScale2D(scale);
    }
}
[HarmonyPatch(typeof(Knight.HeroController), nameof(Knight.HeroController.DoDoubleJump), MethodType.Normal)]
public class Patch_Knight_HeroController_DoDoubleJump : GeneralPatch
{
    public static void Postfix(Knight.HeroController __instance)
    {
        if (Knight.PlayerData.instance.infiniteAirJump)
            __instance.doubleJumped = false;
    }
}