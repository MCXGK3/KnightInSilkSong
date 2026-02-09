using KIS;

[HarmonyPatch(typeof(CustomSceneManager), nameof(CustomSceneManager.Start), MethodType.Normal)]
public class Patch_CustomSceneManager_Start : GeneralPatch
{
    public static bool Prefix(CustomSceneManager __instance)
    {

        if ((PlayerData.instance.HeroCorpseType & HelperFun.knight_death_cocoon) != GlobalEnums.HeroDeathCocoonTypes.Normal)
        {
            __instance.heroCorpsePrefab = KnightInSilksong.loaded_gos["Hollow Shade"];
        }
        return true;
    }
    public static void Postfix(CustomSceneManager __instance)
    {
    }
}