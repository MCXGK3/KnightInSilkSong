using TeamCherry.Localization;

[HarmonyPatch(typeof(Language), nameof(Language.Get), [typeof(string), typeof(string)])]
public static class Patch_Language_Get
{
    public static bool Prefix(string key, string sheetTitle, ref string __result)
    {
        if (MoreLanguge.langs.TryGetValue((sheetTitle, key), out var val))
        {
            __result = val;
            return false;
        }
        return true;
    }
}

