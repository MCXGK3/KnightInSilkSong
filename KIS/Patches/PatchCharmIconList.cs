using KIS;

[HarmonyPatch(typeof(CharmIconList), "GetSprite", MethodType.Normal)]
public class Patch_CharmIconList_GetSprite : GeneralPatch
{
    public static bool Prefix(CharmIconList __instance, int id, ref Sprite __result)
    {
        if (KnightInSilksong.IsKnight)
        {
            Knight.PlayerData pd = Knight.PlayerData.instance;
            switch (id)
            {
                case 23:
                    if (pd.fragileHealth_unbreakable)
                    {
                        __result = __instance.unbreakableHeart;
                        return false;
                    }
                    break;
                case 24:
                    if (pd.fragileGreed_unbreakable)
                    {
                        __result = __instance.unbreakableGreed;
                        return false;
                    }
                    break;
                case 25:
                    if (pd.fragileStrength_unbreakable)
                    {
                        __result = __instance.unbreakableStrength;
                        return false;
                    }
                    break;
                case 40:
                    switch (pd.grimmChildLevel)
                    {
                        case 1:
                            __result = __instance.grimmchildLevel1;
                            return false;
                        case 2:
                            __result = __instance.grimmchildLevel2;
                            return false;
                        case 3:
                            __result = __instance.grimmchildLevel3;
                            return false;
                        case 4:
                            __result = __instance.grimmchildLevel4;
                            return false;
                        case 5:
                            __result = __instance.nymmCharm;
                            return false;
                    }
                    break;
            }
        }
        return true;
    }
    public static void Postfix(CharmIconList __instance, int id)
    {
    }
}