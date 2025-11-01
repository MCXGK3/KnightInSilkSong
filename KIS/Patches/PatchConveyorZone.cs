using KIS;

[HarmonyPatch(typeof(ConveyorZone), "OnTriggerEnter2D", MethodType.Normal)]
public class Patch_ConveyorZone_OnTriggerEnter2D : GeneralPatch
{
    public static bool Prefix(ConveyorZone __instance, Collider2D collision)
    {
        if (KnightInSilksong.IsKnight)
        {
            if (!__instance.activated)
            {
                return true;
            }

            __instance.hasEntered = true;
            __instance.conveyorMovement = collision.GetComponent<ConveyorMovement>();
            if ((bool)__instance.conveyorMovement)
            {
                __instance.conveyorMovement.StartConveyorMove(__instance.speed, 0f);
            }

            var hc = collision.GetComponent<Knight.HeroController>();
            if ((bool)hc)
            {
                if (__instance.vertical)
                {
                    hc.GetComponent<ConveyorMovementHero>().StartConveyorMove(0f, __instance.speed);
                    hc.cState.onConveyorV = true;
                }
                else
                {
                    hc.SetConveyorSpeed(__instance.speed);
                    hc.cState.inConveyorZone = true;
                }
            }
            return false;
        }
        return true;
    }
    public static void Postfix(ConveyorZone __instance, Collider2D collision)
    {
    }
}
[HarmonyPatch(typeof(ConveyorZone), "OnTriggerExit2D", MethodType.Normal)]
public class Patch_ConveyorZone_OnTriggerExit2D : GeneralPatch
{
    public static bool Prefix(ConveyorZone __instance, Collider2D collision)
    {
        if (KnightInSilksong.IsKnight)
        {
            if (__instance.activated)
            {
                __instance.hasEntered = false;
                ConveyorMovement component = collision.GetComponent<ConveyorMovement>();
                if ((bool)component)
                {
                    component.StopConveyorMove();
                }

                Knight.HeroController component2 = collision.GetComponent<Knight.HeroController>();
                if ((bool)component2)
                {
                    component2.GetComponent<ConveyorMovementHero>().StopConveyorMove();
                    component2.cState.inConveyorZone = false;
                    component2.cState.onConveyorV = false;
                }
            }
            return false;
        }
        return true;
    }
    public static void Postfix(ConveyorZone __instance, Collider2D collision)
    {
    }
}
[HarmonyPatch(typeof(ConveyorZone), "OnDisable", MethodType.Normal)]
public class Patch_ConveyorZone_OnDisable : GeneralPatch
{
    public static bool Prefix(ConveyorZone __instance)
    {
        if (KnightInSilksong.IsKnight)
        {
            if (__instance.hasEntered)
            {
                if ((bool)__instance.conveyorMovement)
                {
                    __instance.conveyorMovement.StopConveyorMove();
                }


                var hc = Knight.HeroController.instance;
                if ((bool)hc)
                {
                    hc.GetComponent<ConveyorMovementHero>().StopConveyorMove();
                    hc.cState.inConveyorZone = false;
                    hc.cState.onConveyorV = false;
                }


                __instance.hasEntered = false;
                __instance.conveyorMovement = null;
                __instance.hc = null;
            }
        }
        return true;
    }
    public static void Postfix(ConveyorZone __instance)
    {
    }
}