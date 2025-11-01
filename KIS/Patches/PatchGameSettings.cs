using KIS;
using TeamCherry.SharedUtils;

[HarmonyPatch(typeof(GameSettings), "LoadAudioSettings", MethodType.Normal)]
public class Patch_GameSettings_LoadAudioSettings : GeneralPatch
{
    public static bool Prefix(GameSettings __instance)
    {
        return true;
    }
    public static void Postfix(GameSettings __instance)
    {
        var master = KnightInSilksong.Master;
        float master_level = __instance.masterVolume;
        float sound_level = __instance.soundVolume;
        float value = global::Helper.LinearToDecibel(master_level / 10f);
        master.SetFloat("MasterVolume", value);
        value = global::Helper.LinearToDecibel(sound_level / 10f);
        master.SetFloat("SFXVolume", value);
        "LoadAudioSettings OK".LogInfo();

    }
}