using KIS;

[HarmonyPatch(typeof(CharmDisplay), "Start", MethodType.Normal)]
public class Patch_CharmDisplay_Start : GeneralPatch
{
    public static bool Prefix(CharmDisplay __instance)
    {
        return true;
    }
    public static void Postfix(CharmDisplay __instance)
    {
        if (KnightInSilksong.IsKnight)
        {


            Sprite sprite = null;
            Knight.PlayerData pd = Knight.PlayerData.instance;
            switch (__instance.id)
            {
                case 23:
                    if (pd.brokenCharm_23) sprite = __instance.brokenGlassHP;
                    break;
                case 24:
                    if (pd.brokenCharm_24) sprite = __instance.brokenGlassGeo;
                    break;
                case 25:
                    if (pd.brokenCharm_25) sprite = __instance.brokenGlassAttack;
                    break;
                case 36:
                    if (pd.royalCharmState > 3) sprite = __instance.blackCharm;
                    else sprite = __instance.whiteCharm;
                    break;
            }
            if (sprite != null)
            {
                if ((bool)__instance.spriteRenderer)
                    __instance.spriteRenderer.sprite = sprite;
                if ((bool)__instance.flashSpriteRenderer)
                    __instance.flashSpriteRenderer.sprite = sprite;
            }
        }
    }
}