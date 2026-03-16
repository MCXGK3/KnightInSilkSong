using KIS;
using TeamCherry.Localization;

[HarmonyPatch(typeof(Language), nameof(Language.Get), [typeof(string), typeof(string)])]
public class Patch_Language_Get : StartPatch
{
    private static bool TextFromKnight(string key, string sheetTitle)
    {
        if (sheetTitle == "UI" &&
            (key.StartsWith("CHARM_NAME_") ||
                key.StartsWith("CHARM_DESC_") ||
                key.StartsWith("CHARM_TXT_")))
        {
            return true;
        }
        return false;
    }
    public static bool Prefix(string key, ref string sheetTitle, ref string __result)
    {
        if (TextFromKnight(key, sheetTitle))
        {
            sheetTitle = $"Mods.{KnightInSilksong.Id}";
        }
        return true;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Language), nameof(Language.Has), MethodType.Normal)]
    public static bool Language_Has_Prefix(ref string key, ref string sheetTitle)
    {
        if (TextFromKnight(key, sheetTitle))
        {
            sheetTitle = $"Mods.{KnightInSilksong.Id}";
        }
        return true;
    }
}


