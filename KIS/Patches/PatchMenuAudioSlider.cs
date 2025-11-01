using KIS;
using UnityEngine.Audio;

[HarmonyPatch(typeof(MenuAudioSlider), "SetSoundLevel", MethodType.Normal)]
public class Patch_MenuAudioSlider_SetSoundLevel : GeneralPatch
{

    public static bool Prefix(MenuAudioSlider __instance, float soundLevel)
    {
        return true;
    }
    public static void Postfix(MenuAudioSlider __instance, float soundLevel)
    {
        ("Sound Volume " + __instance.gs.soundVolume).LogInfo();
        var master = KnightInSilksong.Master;
        if (master == null) return;
        float value = global::Helper.LinearToDecibel(__instance.GetVolumeLevel(soundLevel));
        master.SetFloat("SFXVolume", value);
    }
}
[HarmonyPatch(typeof(MenuAudioSlider), "SetMasterLevel", MethodType.Normal)]
public class Patch_MenuAudioSlider_SetMasterLevel : GeneralPatch
{
    public static bool Prefix(MenuAudioSlider __instance, float masterLevel)
    {
        return true;
    }
    public static void Postfix(MenuAudioSlider __instance, float masterLevel)
    {
        ("Master Volume " + __instance.gs.soundVolume).LogInfo();
        var master = KnightInSilksong.Master;
        if (master == null) return;
        float value = global::Helper.LinearToDecibel(__instance.GetVolumeLevel(masterLevel));
        master.SetFloat("MasterVolume", value);
    }
}