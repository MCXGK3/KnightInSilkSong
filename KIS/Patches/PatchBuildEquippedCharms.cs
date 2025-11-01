using KIS;

[HarmonyPatch(typeof(BuildEquippedCharms), "BuildCharmList", MethodType.Normal)]
public class Patch_BuildEquippedCharms_BuildCharmList : GeneralPatch
{
    public static bool Prefix(BuildEquippedCharms __instance)
    {
        if (KnightInSilksong.IsKnight)
        {
            __instance.equippedCharms = Knight.PlayerData.instance.equippedCharms;
            // Knight.PlayerData.instance.CalculateNotchesUsed();
        }
        return true;
    }
    public static void Postfix(BuildEquippedCharms __instance)
    {
        if (KnightInSilksong.IsKnight)
        {
            if (Knight.PlayerData.instance.charmSlotsFilled >= Knight.PlayerData.instance.charmSlots)
            {
                try
                {
                    __instance.instanceList.Remove(__instance.nextDot);
                }
                catch { }

            }
        }

    }
}