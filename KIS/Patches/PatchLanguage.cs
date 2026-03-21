using KIS;
using TeamCherry.Localization;

[HarmonyPatch(typeof(Language), nameof(Language.Get), [typeof(string), typeof(string)])]
public class Patch_Language_Get : StartPatch
{
    private static void CheckText(ref string key, ref string sheetTitle)
    {
        if (sheetTitle == "UI" &&
            (key.StartsWith("CHARM_NAME_") ||
                key.StartsWith("CHARM_DESC_") ||
                key.StartsWith("CHARM_TXT_")))
        {
            sheetTitle = $"Mods.{KnightInSilksong.Id}";
        }

        return;
    }
    public static bool Prefix(ref string key, ref string sheetTitle, ref string __result)
    {
        CheckText(ref key, ref sheetTitle);
        return true;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Language), nameof(Language.Has), MethodType.Normal)]
    public static bool Language_Has_Prefix(ref string key, ref string sheetTitle)
    {
        CheckText(ref key, ref sheetTitle);
        return true;
    }
}


