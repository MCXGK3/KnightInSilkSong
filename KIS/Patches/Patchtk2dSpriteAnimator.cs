using KIS;

[HarmonyPatch(typeof(tk2dSpriteAnimator), "GetClipByName", MethodType.Normal)]
public class Patch_tk2dSpriteAnimator_GetClipByName : GeneralPatch
{
    public static Dictionary<string, string> hornet_to_knight_anime_with_event = new()
    {
        {"HardLand Greymoor","HardLand"}


    };

    public static bool Prefix(tk2dSpriteAnimator __instance, ref string name)
    {
        if (KnightInSilksong.IsKnight)
        {
            if (__instance.gameObject == Knight.HeroController.instance.gameObject)
            {
                if (hornet_to_knight_anime_with_event.ContainsKey(name))
                {
                    name = hornet_to_knight_anime_with_event[name];
                }
            }
        }
        return true;
    }
    public static void Postfix(tk2dSpriteAnimator __instance, string name)
    {
    }
}