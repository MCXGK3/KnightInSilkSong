using HutongGames.PlayMaker.Actions;

namespace KIS;

public enum LangKey
{
    DEBUG_SCREAM_NAME,
    DEBUG_FIREBALL_NAME,
    DEBUG_QUAKE_NAME,
    KING_SOUL,
    GRIMM_CHILD,
    DEBUG_ALL_CHARMS,
    DEBUG_REMOVE_ALL_CHARMS,
    OVERCHARM,
    DEBUG_INCREASE_CHARMSLOTS,
    DEBUG_DECREASE_CHARMSLOTS,
    DEBUG_ALL_SKILL,
    DEBUG_DASH_NAME,
    DEBUG_DOUBLE_JUMP_NAME,
    DEBUG_WALL_JUMP_NAME,
    DEBUG_SUPER_DASH_NAME,
    DEBUG_ACID_SWIM_NAME,
    DEBUG_DREAM_NAIL_NAME,
    DEBUG_DREAM_GATE_NAME,
    DEBUG_GREAT_SLASH_NAME,
    DEBUG_DASH_SLASH_NAME,
    DEBUG_CYCLONE_NAME,
    DEBUG_PROMPT

}

public class MoreLanguge : IModule
{
    private static Dictionary<LangKey, string> game_key_dict = new()
    {
        {LangKey.KING_SOUL,"CHARM_NAME_36_B"},
        {LangKey.GRIMM_CHILD,"CHARM_NAME_40"},
        {LangKey.OVERCHARM,"CHARM_TXT_OVERCHARMED"},
        {LangKey.DEBUG_DOUBLE_JUMP_NAME,"INV_NAME_DOUBLEJUMP"},
        {LangKey.DEBUG_WALL_JUMP_NAME,"INV_NAME_WALLJUMP"},
        {LangKey.DEBUG_SUPER_DASH_NAME,"INV_NAME_SUPERDASH"},
        {LangKey.DEBUG_ACID_SWIM_NAME,"INV_NAME_ACIDARMOUR"},
        {LangKey.DEBUG_DREAM_NAIL_NAME,"INV_NAME_DREAMNAIL_A"},
        {LangKey.DEBUG_DREAM_GATE_NAME,"INV_NAME_DREAMGATE"},
        {LangKey.DEBUG_DASH_SLASH_NAME,"INV_NAME_ART_UPPER"},
        {LangKey.DEBUG_GREAT_SLASH_NAME,"INV_NAME_ART_DASH"},
        {LangKey.DEBUG_CYCLONE_NAME,"INV_NAME_ART_CYCLONE"}
    };
    private readonly List<string> custom_keys = [
        "DEBUG_FIREBALL_NAME",
        "DEBUG_SCREAM_NAME",
        "DEBUG_QUAKE_NAME",
        "DEBUG_ALL_CHARMS",
        "DEBUG_REMOVE_ALL_CHARMS",
        "DEBUG_INCREASE_CHARMSLOTS",
        "DEBUG_DECREASE_CHARMSLOTS",
        "DEBUG_ALL_SKILL",
        "DEBUG_DASH_NAME",
        "DEBUG_PROMPT"
    ];
    public static string GetInGameKey(LangKey langKey)
    {
        return game_key_dict.TryGetValue(langKey, out string res) ? res : langKey.ToString();
    }
    public override void Init()
    {
        base.Init();
    }
}