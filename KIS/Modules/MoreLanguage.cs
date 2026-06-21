using HutongGames.PlayMaker.Actions;
using TeamCherry.Localization;

namespace KIS;

public enum LangKey
{
    DEBUG_SCREAM_NAME,
    DEBUG_FIREBALL_NAME,
    DEBUG_QUAKE_NAME,
    KING_SOUL,
    GRIMM_CHILD,
    CHARM_HEART,
    CHARM_GREED,
    CHARM_STRENGTH,
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
    DEBUG_PROMPT,
    DEBUG_FIX_CHARMSLOTS,
    MM_MAX_HEALTH_BASE,
    MM_NAIL_UPGRADES,
    MM_KNIGHT_NAIL_UPGRADES,
    MM_BOSSRUSH_MODE,
    MM_UNLOCK_SLOTS,
    MM_SILKSHOT_ARCHITECT,
    MM_SILKSHOT_FORGE,
    MM_SILKSHOT_WEAVER,
    DASH_NAME,
    SHADOWDASH_NAME,
    SALUBRA_NAME,
    CHARMSLOT_NAME,
    MAP_NAME,
    SYNC_MODE_SET,
    SYNC_MODE_EQUAL,
    SYNC_MODE_CONTRIBUTE,
    MM_ROYAL_CHARM_STATE,
    MM_GRIMM_CHARM_STATE,
    WHITE_FRAGMENT,
    VOID_HEART,
    CAREFREE_MELODY,
    SOUL_ORB,
    MM_ADD,
    MM_DELETE,
    MM_RELOAD,
    MM_DEFAULT,
    MM_EDIT,
    MM_CLEAR,
    MM_VALIDATE,
    MM_SAVE,
    MM_OK,
    MM_FAIL,
    MM_MODDED,
    MM_STATE,
    MM_SAVE_OPTIONS,
    MM_ALLOW_LOG,
    MM_TOGGLE_BUTTON,
    MM_APPLY_DAMAGE_SCALING,
    MM_DEFAULT_SYNC,
    MM_KNIGHT_SCALE_X,
    MM_KNIGHT_SCALE_Y,
    MM_SYNC

}

public class MoreLanguge : IModule
{
    public static LocalisedString Yes => new("MainMenu", "NAV_YES");
    public static LocalisedString No => new("MainMenu", "NAV_NO");
    public static LocalisedString On => new("MainMenu", "MOH_ON");
    public static LocalisedString Off => new("MainMenu", "MOH_OFF");
    public static LocalisedString None => new("MainMenu", "KEYBOARD_NONE");
    private static Dictionary<LangKey, string> game_key_dict = new()
    {
        {LangKey.KING_SOUL,"CHARM_NAME_36_B"},
        {LangKey.GRIMM_CHILD,"CHARM_NAME_40"},
        {LangKey.CHARM_HEART,"CHARM_NAME_23_G"},
        {LangKey.CHARM_GREED,"CHARM_NAME_24_G"},
        {LangKey.CHARM_STRENGTH,"CHARM_NAME_25_G"},
        {LangKey.OVERCHARM,"CHARM_TXT_OVERCHARMED"},
        {LangKey.DEBUG_DOUBLE_JUMP_NAME,"INV_NAME_DOUBLEJUMP"},
        {LangKey.DEBUG_WALL_JUMP_NAME,"INV_NAME_WALLJUMP"},
        {LangKey.DEBUG_SUPER_DASH_NAME,"INV_NAME_SUPERDASH"},
        {LangKey.DEBUG_ACID_SWIM_NAME,"INV_NAME_ACIDARMOUR"},
        {LangKey.DEBUG_DREAM_NAIL_NAME,"INV_NAME_DREAMNAIL_A"},
        {LangKey.DEBUG_DREAM_GATE_NAME,"INV_NAME_DREAMGATE"},
        {LangKey.DEBUG_DASH_SLASH_NAME,"INV_NAME_ART_UPPER"},
        {LangKey.DEBUG_GREAT_SLASH_NAME,"INV_NAME_ART_DASH"},
        {LangKey.DEBUG_CYCLONE_NAME,"INV_NAME_ART_CYCLONE"},
        {LangKey.DASH_NAME,"INV_NAME_DASH"},
        {LangKey.SHADOWDASH_NAME,"INV_NAME_SHADOWDASH"},
        {LangKey.SALUBRA_NAME,"NAME_BLESSING"},
        {LangKey.CHARMSLOT_NAME,"INV_NAME_NOTCH"},
        {LangKey.MAP_NAME,"PANE_MAP"},
        {LangKey.WHITE_FRAGMENT,"CHARM_NAME_36_A"},
        {LangKey.VOID_HEART,"CHARM_NAME_36_C"},
        {LangKey.CAREFREE_MELODY,"CHARM_NAME_40_N"},
        {LangKey.SOUL_ORB,"INV_NAME_SOULORBS_ALL"}
    };
    public static string GetInGameKey(LangKey langKey)
    {
        return game_key_dict.TryGetValue(langKey, out string res) ? res : langKey.ToString();
    }
    public override void Init()
    {
        base.Init();
    }
}