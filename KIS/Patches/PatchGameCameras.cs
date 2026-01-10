using KIS;

[HarmonyPatch(typeof(GameCameras), "HUDOut", MethodType.Normal)]
public class Patch_GameCameras_HUDOut : GeneralPatch
{
    public static void Postfix(GameCameras __instance)
    {
        if (KnightInSilksong.IsKnight)
        {
            FSMUtility.SendEventToGameObject(KnightInSilksong.Instance.hud_instance, "OUT");
            "Hud OUT".LogInfo();

        }
    }
}
[HarmonyPatch(typeof(GameCameras), "HUDIn", MethodType.Normal)]
public class Patch_GameCameras_HUDIn : GeneralPatch
{
    public static void Postfix(GameCameras __instance)
    {
        if (KnightInSilksong.IsKnight)
        {
            FSMUtility.SendEventToGameObject(KnightInSilksong.Instance.hud_instance, "IN");
            "Hud IN".LogInfo();
        }
    }
}
