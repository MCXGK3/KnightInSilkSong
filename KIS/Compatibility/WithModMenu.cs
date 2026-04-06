using System.Diagnostics.CodeAnalysis;
using System.IO;
using AsmResolver.IO;
using BepInEx;
using BepInEx.Configuration;
using GlobalEnums;
using GlobalSettings;
using HutongGames.PlayMaker.Actions;
using KIS.Compatibility;
using Newtonsoft.Json;
using PolyAndCode.UI;
using Silksong.ModMenu;
using Silksong.ModMenu.Elements;
using Silksong.ModMenu.Internal;
using Silksong.ModMenu.Models;
using Silksong.ModMenu.Plugin;
using Silksong.ModMenu.Screens;
using TeamCherry.Localization;
using Unity.Collections;
using UnityEngine.Pool;
using UnityEngine.UI;
using UnityEngine.UIElements.UIR;
using static KIS.SyncManager;
using KP = KIS.KnightInSilksong;

namespace KIS.Compatibility;

internal class ModMenuIgnoreAttribute : System.Attribute { }



public class ScrollContent : AbstractGroup
{
    public readonly List<(IMenuEntity, Func<IMenuEntity, float>, Func<IMenuEntity, float>)> entities_with_height = new();
    public readonly List<IMenuEntity> entities = new();
    static GameObject scoll_content_prefab;
    public static GameObject GetPrefab()
    {
        if (scoll_content_prefab == null)
        {
            var content = UIManager.instance.UICanvas.transform.Find("AchievementsMenuScreen/Content").gameObject;
            content.SetActive(false);
            var new_go = GameObject.Instantiate(content);
            content.SetActive(true);
            var scroll_rect = new_go.Find("ScrollRect");
            var ach_list = new_go.transform.Find("ScrollRect/AchievementListUI").gameObject;
            var scroll_bar = new_go.Find("Scrollbar");
            scroll_bar.GetComponent<Scrollbar>().onValueChanged = new();
            scroll_rect.RemoveComponent<RecyclableScrollRect>();
            scroll_rect.RemoveComponent<RectMask2D>();
            ach_list.RemoveComponent<MenuAchievementsList>();
            ach_list.RemoveComponent<GridLayoutGroup>();
            ach_list.DestroyAllChildren();
            ach_list.name = "ViewPort";
            ach_list.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
            ach_list.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 0f);
            ach_list.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 1f);
            ach_list.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            ach_list.GetComponent<RectTransform>().sizeDelta = scroll_rect.GetComponent<RectTransform>().sizeDelta;
            var mask = ach_list.AddComponent<RectMask2D>();
            var true_content = new GameObject("Content");
            true_content.AddComponent<RectTransform>();
            true_content.transform.SetParent(ach_list.transform);
            var true_content_rect = true_content.GetComponent<RectTransform>();
            true_content_rect.anchoredPosition = Vector2.zero;
            true_content_rect.anchorMin = new Vector2(0f, 1f);
            true_content_rect.anchorMax = new Vector2(1f, 1f);
            true_content_rect.pivot = new Vector2(0.5f, 1f);
            // true_content.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            var scroll_rect_com = scroll_rect.AddComponent<ScrollRect>();
            scroll_rect_com.scrollSensitivity = 40f;
            scroll_rect_com.movementType = ScrollRect.MovementType.Clamped;
            scroll_rect_com.vertical = true;
            scroll_rect_com.horizontal = false;
            scroll_rect_com.verticalScrollbar = scroll_bar.GetComponent<Scrollbar>();
            scroll_rect_com.viewport = ach_list.transform as RectTransform;
            scroll_rect_com.content = true_content.transform as RectTransform;
            // ach_list.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            new_go.SetActive(false);
            new_go.name = "ScollContent";
            GameObject.DontDestroyOnLoad(new_go);
            scoll_content_prefab = new_go;
        }
        return GameObject.Instantiate(scoll_content_prefab);
    }

    public override IEnumerable<IMenuEntity> AllEntities()
    {
        return entities;
    }

    public override IEnumerable<INavigable> GetNavigables(NavigationDirection direction)
    {
        switch (direction)
        {
            case NavigationDirection.Left:
            case NavigationDirection.Right:
                return NonHiddenEntities().OfType<INavigable>();
            case NavigationDirection.Up:
                return NonHiddenEntities().OfType<INavigable>().Take(1);
            case NavigationDirection.Down:
                return NonHiddenEntities().OfType<INavigable>().Reverse().Take(1);
            default:
                throw direction.UnsupportedEnum();
        }
    }

    public override bool GetSelectable(NavigationDirection direction, [MaybeNullWhen(false)] out Selectable selectable)
    {
        switch (direction)
        {
            case NavigationDirection.Left:
            case NavigationDirection.Right:
                {
                    selectable = (from n in NonHiddenEntities().MedianOutwards().OfType<INavigable>()
                                  select (!n.GetSelectable(direction, out Selectable selectable2)) ? null : selectable2).FirstOrDefault();
                    return selectable != null;
                }
            case NavigationDirection.Up:
                {
                    selectable = (from n in NonHiddenEntities().OfType<INavigable>()
                                  select (!n.GetSelectable(direction, out Selectable selectable2)) ? null : selectable2).LastOrDefault();
                    return selectable != null;
                }
            case NavigationDirection.Down:
                {
                    selectable = (from n in NonHiddenEntities().OfType<INavigable>()
                                  select (!n.GetSelectable(direction, out Selectable selectable2)) ? null : selectable2).FirstOrDefault();
                    return selectable != null;
                }
            default:
                throw direction.UnsupportedEnum();
        }
    }
    public IEnumerable<IMenuEntity> NonHiddenEntities()
    {
        if (!HideInactiveElements)
        {
            return entities;
        }

        return entities.Where((IMenuEntity e) => IMenuEntityExtensions.get_VisibleSelf(e));
    }

    public override void UpdateLayout(Vector2 localAnchorPos)
    {
        BeforeUpdate?.Invoke();
        ClearNeighbors();
        content.GetComponent<RectTransform>().SetAnchoredPosition(localAnchorPos);
        true_content.GetComponent<RectTransform>().sizeDelta = new(0, entities_with_height.Sum((e) => e.Item2(e.Item1)));
        Vector2 ini_pos = true_content.GetComponent<RectTransform>().sizeDelta / 2;
        foreach (var entity_pair in entities_with_height)
        {
            if (entity_pair.Item3 != null)
            {
                ini_pos.y -= entity_pair.Item3(entity_pair.Item1);
                entity_pair.Item1.UpdateLayout(ini_pos);
                ini_pos.y -= entity_pair.Item2(entity_pair.Item1) - entity_pair.Item3(entity_pair.Item1);
                continue;
            }
            else
            {
                ini_pos.y -= entity_pair.Item2(entity_pair.Item1) / 2;
                entity_pair.Item1.UpdateLayout(ini_pos);
                ini_pos.y -= entity_pair.Item2(entity_pair.Item1) / 2;
            }
        }
        if (true_content.GetComponent<RectTransform>().sizeDelta.y <= view_port.GetComponent<RectTransform>().rect.height)
        {
            scroll_rect_component.verticalScrollbar.gameObject.SetActive(false);
            scroll_rect_component.vertical = false;
        }
        else
        {
            scroll_rect_component.verticalScrollbar.gameObject.SetActive(true);
            scroll_rect_component.vertical = true;
            scroll_rect_component.verticalScrollbar = scroll_rect_component.verticalScrollbar;
        }


    }
    public bool HideInactiveElements = true;
    public GameObject content;
    public GameObject scroll_rect;
    public ScrollRect scroll_rect_component;
    public GameObject view_port;
    public GameObject gameobject_parent;
    public GameObject true_content;
    public float VerticalSpacing = SpacingConstants.VSPACE_MEDIUM;
    public float height;
    public float width;
    public Action BeforeUpdate = null;

    static void SetRectTransformHeight(RectTransform rect, float height)
    {
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, height);
    }
    static void SetRectTransformWidth(RectTransform rect, float width)
    {
        rect.sizeDelta = new Vector2(width, rect.sizeDelta.y);
    }

    public ScrollContent(float height = 906f, float width = 2000f, Color? color = null) : base()
    {
        this.height = height;
        content = GetPrefab();
        scroll_rect = content.transform.Find("ScrollRect").gameObject;
        scroll_rect_component = scroll_rect.GetComponent<ScrollRect>();
        view_port = scroll_rect.Find("ViewPort").gameObject;
        true_content = view_port.transform.Find("Content").gameObject;
        true_content.AddComponent<Image>().color = color ?? new Color(0, 0, 0, 0);
        visibility.OnVisibilityChanged += content.SetActive;
        content.SetActive(true);
        content.transform.SetScale2D(Vector2.one);
        view_port.GetComponent<RectTransform>().anchorMin = Vector2.zero;
        view_port.GetComponent<RectTransform>().anchorMax = Vector2.one;
        true_content.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 1f);
        true_content.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 1f);
        true_content.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 1f);
        true_content.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        SetRectTransformHeight(content.GetComponent<RectTransform>(), height);
        SetRectTransformWidth(content.GetComponent<RectTransform>(), width);
        SetRectTransformHeight(content.transform.Find("Scrollbar") as RectTransform, height);
        SetRectTransformHeight(content.transform.Find("Scrollbar/Background") as RectTransform, height);

    }
    public override void ClearGameObjectParent()
    {
        if (gameObjectParent == null)
        {
            return;
        }
        content.transform.SetParent(null);
        content.SetActive(false);
    }
    public override void SetGameObjectParent(GameObject parent)
    {
        ClearGameObjectParent();
        gameobject_parent = parent;
        content.transform.SetParent(gameobject_parent.transform);
        content.transform.SetScale2D(Vector2.one);

    }
    public new void AddChild(IMenuEntity entity)
    {
        entity.SetParents(this, true_content);
    }
    public void Add(IMenuEntity entity, Func<IMenuEntity, float> height_func = null, Func<IMenuEntity, float> top_padding = null)
    {
        entities.Add(entity);
        entities_with_height.Add((entity, height_func ?? (_ => SpacingConstants.VSPACE_MEDIUM), top_padding));
        AddChild(entity);
    }
    public void Insert(int index, IMenuEntity entity, Func<IMenuEntity, float> height_func = null, Func<IMenuEntity, float> top_padding = null)
    {
        entities.Insert(index, entity);
        entities_with_height.Insert(index, (entity, height_func ?? (_ => SpacingConstants.VSPACE_MEDIUM), top_padding));
        AddChild(entity);
    }
    public override bool Contains(IMenuEntity entity)
    {
        return entities.Contains(entity);
    }
    public bool Remove(IMenuEntity entity)
    {
        int index = entities.IndexOf(entity);
        if (index == -1)
        {
            return false;
        }

        entities.RemoveAt(index);
        entities_with_height.RemoveAt(index);
        entity.ClearParents();
        return true;
    }

    public override void Clear()
    {
        foreach (var entity in entities)
        {
            entity.ClearGameObjectParent();
        }
        entities.Clear();
        entities_with_height.Clear();
    }
}

public class EntryLine
{
    static PlayerData hd => PlayerData.instance;
    static Knight.PlayerData kd => Knight.PlayerData.instance;
    public class HornetVariableInfo
    {
        public LocalisedString name;
        public Type type;
        public Func<object, int?, object> saver;
        public Func<object, int?, object> loader;
        public List<object> display_values;
        //只在saver和loader都为false时使用
        public List<object> store_values;
        public string id;

        public object Display2Store(object o, int? index)
        {
            if (saver == null && store_values == null)
            {
                throw new Exception($"You must set either saver or store_values");
            }
            index ??= display_values?.IndexOf(o);
            if (saver != null)
            {
                return saver(o, index);
            }
            else
            {
                return store_values[index.Value];
            }
        }
        public object Store2Display(object o, int? index)
        {
            if (loader == null && display_values == null)
            {
                throw new Exception($"You must set either loader or display_values");
            }
            index ??= store_values?.IndexOf(o);
            if (loader != null)
            {
                return loader(o, index);
            }
            else
            {
                if (index == null)
                {
                    $"{name}:index error".LogWarning();
                    return null;
                }
                try
                {
                    return display_values[index.Value];
                }
                catch
                {
                    $"{name} {index.Value}".LogWarning();
                    return null;
                }

            }
        }

    }
    public class KnightVariableInfo
    {
        public LocalisedString name;
        public Type type;
        public Func<object, int?, object> saver;
        public Func<object, int?, object> loader;
        public List<object> display_values;
        public List<object> store_values;
        public string id;
        public object Display2Store(object o, int? index)
        {
            if (saver == null && store_values == null)
            {
                throw new Exception($"You must set either saver or store_values");
            }
            index ??= display_values?.IndexOf(o);
            if (saver != null)
            {
                return saver(o, index);
            }
            else
            {
                return store_values[index.Value];
            }
        }
        public object Store2Display(object o, int? index)
        {
            if (loader == null && display_values == null)
            {
                throw new Exception($"You must set either loader or display_values");
            }
            index ??= store_values?.FindIndex((e) => e.Equals(o));
            if (loader != null)
            {
                return loader(o, index);
            }
            else
            {
                if (index == null)
                {
                    $"{name.ToString()}:index error".LogWarning();
                    return null;
                }
                try
                {
                    return display_values[index.Value];
                }
                catch
                {
                    $"{name} {index.Value} {o} ".LogWarning();
                    $"{o.GetType()} {store_values[0].GetType()}".LogWarning();
                    foreach (var value in store_values)
                    {
                        $"{value} \n".LogWarning();
                    }
                    return null;
                }

            }
        }
    }
    public static HornetVariableInfo GeneralHornetInfo<T>(string id, LocalisedString? name = null, List<object> display_values = null, List<object> store_values = null, Func<object, int?, object> saver = null, Func<object, int?, object> loader = null)
    {
        Type type = typeof(T);
        display_values ??= type == typeof(bool) ? [MoreLanguge.Yes, MoreLanguge.No] : null;
        store_values ??= type == typeof(bool) ? [true, false] : null;
        return new HornetVariableInfo()
        {
            name = name ?? new LocalisedString("", id),
            type = type,
            saver = saver,
            loader = loader,
            display_values = display_values,
            store_values = store_values,
            id = id
        };
    }
    public static HornetVariableInfo ToolHornetInfo(Tool tool, LocalisedString? name = null)
    {
        return new HornetVariableInfo()
        {
            name = name ?? tool.Localize(),
            type = typeof(bool),
            saver = null,
            loader = null,
            display_values = [MoreLanguge.Yes, MoreLanguge.No],
            store_values = [true, false],
            id = FromTool.prefix + tool.GetToolName()
        };
    }
    public static List<HornetVariableInfo> AcceptableHornetVariables = new List<HornetVariableInfo>()
    {
        GeneralHornetInfo<bool>(nameof(hd.hasDash),new("UI","INV_NAME_SKILL_SPRINT")),
        GeneralHornetInfo<bool>(nameof(hd.hasWalljump),new("UI","INV_NAME_WALLJUMP")),
        GeneralHornetInfo<bool>(nameof(hd.hasDoubleJump),new("UI","INV_NAME_DRESS_DJ")),
        GeneralHornetInfo<bool>(nameof(hd.hasHarpoonDash),new("UI","INV_NAME_SKILL_HARPOON")),
        GeneralHornetInfo<bool>(nameof(hd.hasNeedleThrow),new("UI","INV_NAME_SKILL_THROW")),
        GeneralHornetInfo<bool>(nameof(hd.hasSilkCharge),new("UI","INV_NAME_SKILL_SILKDASH")),
        GeneralHornetInfo<bool>(nameof(hd.hasParry),new("UI","INV_NAME_SKILL_PARRY")),
        GeneralHornetInfo<bool>(nameof(hd.hasSilkBossNeedle),new("UI","INV_NAME_SKILL_SILKBOSS_NEEDLE")),
        GeneralHornetInfo<bool>(nameof(hd.hasThreadSphere),new("UI","INV_NAME_SKILL_SPHERE")),
        GeneralHornetInfo<bool>(nameof(hd.hasSilkBomb),new("UI","INV_NAME_SKILL_SILKBOMB")),
        GeneralHornetInfo<int>(nameof(hd.maxHealthBase),LangKey.MM_MAX_HEALTH_BASE.Localize(),display_values: [5,6,7,8,9,10],store_values:[5,6,7,8,9,10]),
        GeneralHornetInfo<int>(nameof(hd.nailUpgrades),LangKey.MM_NAIL_UPGRADES.Localize(), display_values: [0, 1, 2, 3, 4],store_values:[5,6,7,8,9,10]),
        GeneralHornetInfo<bool>(nameof(hd.hasNeedolin),new("UI","INV_NAME_SKILL_NEEDOLIN")),
        GeneralHornetInfo<bool>(nameof(hd.UnlockedFastTravelTeleport),new("UI","INV_NAME_SKILL_BELLBEAST_MELODY")),
        GeneralHornetInfo<int>(nameof(hd.permadeathMode),new("MainMenu","MODE_STEEL"),
                            display_values:[MoreLanguge.On,MoreLanguge.Off],
                            store_values:[PermadeathModes.On, PermadeathModes.Off]),
        GeneralHornetInfo<bool>(nameof(hd.bossRushMode),LangKey.MM_BOSSRUSH_MODE.Localize()),
        GeneralHornetInfo<bool>(nameof(hd.HasBoundCrestUpgrader),new("UI","INV_NAME_SKILL_EVAHEAL")),
        GeneralHornetInfo<bool>(nameof(hd.hasChargeSlash),new("UI","INV_NAME_SKILL_CHARGESLASH")),
        GeneralHornetInfo<int>(FromUnlockedSlots.CheckKey,LangKey.MM_UNLOCK_SLOTS.Localize()),

    }.Concat(Enum.GetValues(typeof(Tool)).Cast<Tool>().Where(tool => (int)tool > (int)Tool.Silk_Boss_Needle).Select(tool => ToolHornetInfo(tool))).ToList();

    public static KnightVariableInfo GeneralKnightInfo<T>(string id, LocalisedString? name = null, List<object> display_values = null, List<object> store_values = null, Func<object, int?, object> saver = null, Func<object, int?, object> loader = null)
    {
        var type = typeof(T);
        display_values ??= type == typeof(bool) ? [MoreLanguge.Yes, MoreLanguge.No] : null;
        store_values ??= type == typeof(bool) ? [true, false] : null;
        return new KnightVariableInfo()
        {
            name = name ?? new LocalisedString("", id),
            type = type,
            saver = saver,
            loader = loader,
            display_values = display_values,
            store_values = store_values,
            id = id
        };
    }
    public static KnightVariableInfo CharmKnightInfo(Charm charm, LocalisedString? name = null)
    {
        return new KnightVariableInfo()
        {
            name = name ?? charm.Localize(),
            type = typeof(bool),
            display_values = [MoreLanguge.Yes, MoreLanguge.No],
            store_values = [true, false],
            saver = null,
            loader = null,
            id = charm.GetCharmName()
        };
    }
    public static List<KnightVariableInfo> AcceptableKnightVariables =
    [
        GeneralKnightInfo<bool>(nameof(kd.hasDash),LangKey.DASH_NAME.Localize()),
        GeneralKnightInfo<bool>(nameof(kd.hasShadowDash),LangKey.SHADOWDASH_NAME.Localize()),
        GeneralKnightInfo<bool>(nameof(kd.hasWalljump),LangKey.DEBUG_WALL_JUMP_NAME.Localize()),
        GeneralKnightInfo<bool>(nameof(kd.hasDoubleJump),LangKey.DEBUG_DOUBLE_JUMP_NAME.Localize()),
        GeneralKnightInfo<bool>(nameof(kd.hasSuperDash),LangKey.DEBUG_SUPER_DASH_NAME.Localize()),

        GeneralKnightInfo<int>(nameof(kd.fireballLevel),LangKey.DEBUG_FIREBALL_NAME.Localize(),display_values:[0,1,2],store_values:[0,1,2]),
        GeneralKnightInfo<int>(nameof(kd.quakeLevel),LangKey.DEBUG_QUAKE_NAME.Localize(),display_values:[0,1,2],store_values:[0,1,2]),
        GeneralKnightInfo<int>(nameof(kd.screamLevel),LangKey.DEBUG_SCREAM_NAME.Localize(),display_values:[0,1,2],store_values:[0,1,2]),

        GeneralKnightInfo<int>(nameof(kd.MPReserveMax),LangKey.SOUL_ORB.Localize(),display_values:[0,1,2,3],store_values:[0,33,66,99]),
        GeneralKnightInfo<int>(nameof(kd.maxHealthBase),LangKey.MM_MAX_HEALTH_BASE.Localize(),display_values:[5,6,7,8,9],store_values:[5,6,7,8,9]),
        GeneralKnightInfo<int>(nameof(kd.nailSmithUpgrades),LangKey.MM_KNIGHT_NAIL_UPGRADES.Localize()),

        GeneralKnightInfo<bool>(nameof(kd.hasDreamNail),LangKey.DEBUG_DREAM_NAIL_NAME.Localize()),
        GeneralKnightInfo<bool>(nameof(kd.hasDreamGate),LangKey.DEBUG_DREAM_GATE_NAME.Localize()),
        GeneralKnightInfo<int>(nameof(kd.permadeathMode),new("MainMenu","MODE_STEEL"),display_values:[MoreLanguge.On,MoreLanguge.Off], store_values:[1,0]),
        GeneralKnightInfo<bool>(nameof(kd.bossRushMode),LangKey.MM_BOSSRUSH_MODE.Localize()),
        GeneralKnightInfo<bool>(nameof(kd.salubraBlessing),LangKey.SALUBRA_NAME.Localize()),
        GeneralKnightInfo<bool>(nameof(kd.hasCyclone),LangKey.DEBUG_CYCLONE_NAME.Localize()),
        GeneralKnightInfo<bool>(nameof(kd.hasDashSlash),LangKey.DEBUG_GREAT_SLASH_NAME.Localize()),
        GeneralKnightInfo<bool>(nameof(kd.hasUpwardSlash),LangKey.DEBUG_DASH_SLASH_NAME.Localize()),

        GeneralKnightInfo<bool>(nameof(kd.hasMap),LangKey.MAP_NAME.Localize()),
        GeneralKnightInfo<int>(nameof(kd.charmSlots),LangKey.CHARMSLOT_NAME.Localize(),display_values:[3,4,5,6,7,8,9,10,11],store_values:[3,4,5,6,7,8,9,10,11]),

        .. Enum.GetValues(typeof(Charm)).Cast<Charm>().Select(charm => CharmKnightInfo(charm)),
        GeneralKnightInfo<int>(nameof(kd.royalCharmState),LangKey.MM_ROYAL_CHARM_STATE.Localize(),
                                        display_values:[LangKey.WHITE_FRAGMENT.Localize()+"-1",
                                                        LangKey.WHITE_FRAGMENT.Localize()+"-2",
                                                        LangKey.KING_SOUL.Localize(),
                                                        LangKey.VOID_HEART.Localize()],
                                        store_values:[1,2,3,4]),
        GeneralKnightInfo<int>(nameof(kd.grimmChildLevel),LangKey.MM_GRIMM_CHARM_STATE.Localize(),
                                        display_values:[LangKey.GRIMM_CHILD.Localize()+"-1",
                                                        LangKey.GRIMM_CHILD.Localize()+"-2",
                                                        LangKey.GRIMM_CHILD.Localize()+"-3",
                                                        LangKey.GRIMM_CHILD.Localize()+"-4",
                                                        LangKey.CAREFREE_MELODY.Localize(),
                                                        ],
                                        store_values:[1,2,3,4,5])

    ];
    public static void SetStableTextSize(Text text, int size)
    {
        if (text == null) return;
        var change_scale_com = text.GetComponent<ChangeTextFontScaleOnHandHeld>();
        if (change_scale_com != null) change_scale_com.enabled = false;
        text.fontSize = size;
    }
    List<EntryLine> current_lines;
    internal FreeGroup parentgroup;
    internal GridGroup line;
    ScrollContent scroll;
    TextButton up;
    TextButton down;
    Action update_action;
    internal int index_int = 0;
    TextButton index;
    TextButton hornet;
    HornetVariableInfo current_hornet_info = null;
    TextButton hornet_value;
    object current_hornet_value = null;
    TextButton mode;
    SyncMode? current_mode = null;
    TextButton knight;
    KnightVariableInfo current_knight_info = null;
    TextButton knight_value;
    object current_knight_value = null;
    TextButton add;
    TextButton delete;
    int font_size = 30;
    bool spawned = false;
    internal bool freeze = false;
    bool error = false;
    static List<EntryLine> available_lines = new();

    static GridGroup OptionsBlock
    {
        get
        {
            if (field == null)
            {
                field = new(5);
                field.HorizontalSpacing = SpacingConstants.HSPACE_MEDIUM / 2;
                int num = Math.Max(AcceptableHornetVariables.Count, AcceptableKnightVariables.Count) + 1;
                for (int i = 0; i < num; i++)
                {
                    TextButton button = new("option");
                    field.Add(button);
                }
                field.Visibility.VisibleSelf = false;
            }
            return field;
        }
    }
    internal static TextButton current_selected_button = null;
    internal static EntryLine current_editing_entry = null;
    internal enum LineButtonMode
    {
        HORNET_VARIABLE,
        MODE,
        KNIGHT_VARIABLE,
        HORNET_VALUE,
        KNIGHT_VALUE
    }

    static void SelectLineButton(TextButton button, EntryLine line, LineButtonMode mode)
    {
        if (current_selected_button == button)
        {
            UnSelectLine();
        }
        else
        {
            SelectLine(line);
            current_selected_button = button;
            if (!line.parentgroup.Contains(OptionsBlock))
            {
                line.parentgroup.Add(OptionsBlock, new Vector2(0, -SpacingConstants.VSPACE_SMALL));
            }
            OptionsBlock.Visibility.VisibleSelf = true;
            int count = OptionsBlock.AllEntities().Count();
            int button_font_size = 30;
            switch (mode)
            {
                case LineButtonMode.HORNET_VARIABLE:
                    for (int i = 0; i < count; i++)
                    {
                        if (OptionsBlock.AllEntities().ElementAt(i) is TextButton text_button)
                        {
                            if (i < AcceptableHornetVariables.Count)
                            {
                                var now_info = AcceptableHornetVariables[i];
                                var now_index = i;
                                text_button.ButtonText.text = AcceptableHornetVariables[i].name.ToString();
                                SetStableTextSize(text_button.ButtonText, button_font_size);
                                text_button.Visibility.VisibleSelf = true;
                                text_button.OnSubmit = () => SelectOptionsButton(mode, now_info, now_index);
                                if (current_editing_entry.current_hornet_info == AcceptableHornetVariables[i])
                                {
                                    text_button.State = ElementState.TRUE;
                                    text_button.Interactable = false;
                                }
                                else
                                {
                                    text_button.State = ElementState.FALSE;
                                    text_button.Interactable = true;
                                }
                            }
                            else if (i == AcceptableHornetVariables.Count)
                            {
                                text_button.ButtonText.text = MoreLanguge.None;
                                SetStableTextSize(text_button.ButtonText, button_font_size);
                                text_button.Visibility.VisibleSelf = true;
                                text_button.OnSubmit = () => SelectOptionsButton(mode, null, AcceptableHornetVariables.Count);
                                if (current_editing_entry.current_hornet_info == null)
                                {
                                    text_button.State = ElementState.TRUE;
                                    text_button.Interactable = false;
                                }
                                else
                                {
                                    text_button.State = ElementState.FALSE;
                                    text_button.Interactable = true;
                                }
                            }
                            else
                            {
                                text_button.Visibility.VisibleSelf = false;
                            }

                        }
                    }
                    break;
                case LineButtonMode.KNIGHT_VARIABLE:
                    for (int i = 0; i < count; i++)
                    {
                        if (OptionsBlock.AllEntities().ElementAt(i) is TextButton text_button)
                        {
                            if (i < AcceptableKnightVariables.Count)
                            {
                                var now_info = AcceptableKnightVariables[i];
                                var now_index = i;
                                text_button.ButtonText.text = AcceptableKnightVariables[i].name.ToString();
                                text_button.Visibility.VisibleSelf = true;
                                SetStableTextSize(text_button.ButtonText, button_font_size);
                                text_button.OnSubmit = () => SelectOptionsButton(mode, now_info, now_index);
                                if (current_editing_entry.current_knight_info == AcceptableKnightVariables[i])
                                {
                                    text_button.State = ElementState.TRUE;
                                    text_button.Interactable = false;
                                }
                                else
                                {
                                    text_button.State = ElementState.FALSE;
                                    text_button.Interactable = true;
                                }
                            }
                            else if (i == AcceptableKnightVariables.Count)
                            {
                                text_button.ButtonText.text = MoreLanguge.None;
                                SetStableTextSize(text_button.ButtonText, button_font_size);
                                text_button.Visibility.VisibleSelf = true;
                                text_button.OnSubmit = () => SelectOptionsButton(mode, null, AcceptableKnightVariables.Count);
                                if (current_editing_entry.current_knight_info == null)
                                {
                                    text_button.State = ElementState.TRUE;
                                    text_button.Interactable = false;
                                }
                                else
                                {
                                    text_button.State = ElementState.FALSE;
                                    text_button.Interactable = true;
                                }
                            }
                            else
                            {
                                text_button.Visibility.VisibleSelf = false;
                            }
                        }
                    }
                    break;
                case LineButtonMode.MODE:
                    for (int i = 0; i < count; i++)
                    {
                        if (OptionsBlock.AllEntities().ElementAt(i) is TextButton text_button)
                        {
                            if (i < Enum.GetNames(typeof(SyncMode)).Length)
                            {
                                var now_mode = (SyncMode)Enum.GetValues(typeof(SyncMode)).GetValue(i);
                                var now_index = i;
                                text_button.ButtonText.text = ((LangKey)Enum.Parse(typeof(LangKey), "SYNC_MODE_" + now_mode.ToString())).Localize();
                                text_button.Visibility.VisibleSelf = true;
                                SetStableTextSize(text_button.ButtonText, button_font_size);
                                text_button.OnSubmit = () => SelectOptionsButton(mode, now_mode, now_index);
                                if (current_editing_entry.current_mode == now_mode)
                                {
                                    text_button.State = ElementState.TRUE;
                                    text_button.Interactable = false;
                                }
                                else
                                {
                                    text_button.State = ElementState.FALSE;
                                    text_button.Interactable = true;
                                }
                            }
                            else
                            {
                                text_button.Visibility.VisibleSelf = false;
                            }
                        }
                    }
                    break;
                case LineButtonMode.HORNET_VALUE:
                    if (current_editing_entry.current_hornet_info != null)
                    {
                        List<object> current_display_options = current_editing_entry.current_hornet_info.display_values;
                        List<object> current_store_options = current_editing_entry.current_hornet_info.store_values;
                        for (int i = 0; i < count; i++)
                        {
                            if (OptionsBlock.AllEntities().ElementAt(i) is TextButton text_button)
                            {
                                if (i < current_display_options.Count)
                                {
                                    var now_display_option = current_display_options[i];
                                    var now_store_option = current_store_options[i];
                                    var now_index = i;
                                    text_button.ButtonText.text = now_display_option.ToString();
                                    text_button.Visibility.VisibleSelf = true;
                                    SetStableTextSize(text_button.ButtonText, button_font_size);
                                    text_button.OnSubmit = () => SelectOptionsButton(mode, now_store_option, now_index);
                                    if (current_editing_entry.current_hornet_value != null && Equals(now_store_option, current_editing_entry.current_hornet_value))
                                    {
                                        text_button.State = ElementState.TRUE;
                                        text_button.Interactable = false;
                                    }
                                    else
                                    {
                                        text_button.State = ElementState.FALSE;
                                        text_button.Interactable = true;
                                    }
                                }
                                else
                                {
                                    text_button.Visibility.VisibleSelf = false;
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < count; i++)
                        {
                            if (OptionsBlock.AllEntities().ElementAt(i) is TextButton text_button)
                            {
                                if (i == 0)
                                {
                                    text_button.ButtonText.text = MoreLanguge.None;
                                    text_button.Visibility.VisibleSelf = true;
                                    SetStableTextSize(text_button.ButtonText, button_font_size);
                                    text_button.OnSubmit = null;
                                    text_button.State = ElementState.TRUE;
                                    text_button.Interactable = false;
                                }
                                else
                                {
                                    text_button.Visibility.VisibleSelf = false;
                                }
                            }
                        }
                        $"Error: current hornet variable is null".LogWarning();
                    }
                    break;
                case LineButtonMode.KNIGHT_VALUE:
                    if (current_editing_entry.current_knight_info != null)
                    {
                        List<object> current_display_options = current_editing_entry.current_knight_info.display_values;
                        List<object> current_store_options = current_editing_entry.current_knight_info.store_values;
                        for (int i = 0; i < count; i++)
                        {
                            if (OptionsBlock.AllEntities().ElementAt(i) is TextButton text_button)
                            {
                                if (i < current_display_options.Count)
                                {
                                    var now_display_option = current_display_options[i];
                                    var now_store_option = current_store_options[i];
                                    var now_index = i;
                                    text_button.ButtonText.text = now_display_option.ToString();
                                    text_button.Visibility.VisibleSelf = true;
                                    SetStableTextSize(text_button.ButtonText, button_font_size);
                                    text_button.OnSubmit = () => SelectOptionsButton(mode, now_store_option, now_index);
                                    if (current_editing_entry.current_knight_value != null && Equals(now_store_option, current_editing_entry.current_knight_value))
                                    {
                                        text_button.State = ElementState.TRUE;
                                        text_button.Interactable = false;
                                    }
                                    else
                                    {
                                        text_button.State = ElementState.FALSE;
                                        text_button.Interactable = true;
                                    }
                                }
                                else
                                {
                                    text_button.Visibility.VisibleSelf = false;
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < count; i++)
                        {
                            if (OptionsBlock.AllEntities().ElementAt(i) is TextButton text_button)
                            {
                                if (i == 0)
                                {
                                    text_button.ButtonText.text = MoreLanguge.None;
                                    text_button.Visibility.VisibleSelf = true;
                                    SetStableTextSize(text_button.ButtonText, button_font_size);
                                    text_button.OnSubmit = null;
                                    text_button.State = ElementState.TRUE;
                                    text_button.Interactable = false;
                                }
                                else
                                {
                                    text_button.Visibility.VisibleSelf = false;
                                }
                            }
                        }
                        $"Error: current knight variable is null".LogWarning();
                    }
                    break;
            }
        }
        line.CheckLock();

    }
    static void SelectOptionsButton(LineButtonMode mode, object value, int index)
    {
        if (OptionsBlock == null || current_selected_button == null)
        {
            return;
        }
        switch (mode)
        {
            case LineButtonMode.HORNET_VARIABLE:
                current_editing_entry.current_hornet_info = (HornetVariableInfo)value;
                current_editing_entry.current_hornet_value = null;
                // current_editing_entry.hornet.ButtonText.text = current_editing_entry.current_hornet_info.name.ToString();
                break;
            case LineButtonMode.KNIGHT_VARIABLE:
                current_editing_entry.current_knight_info = (KnightVariableInfo)value;
                current_editing_entry.current_knight_value = null;
                // current_editing_entry.knight.ButtonText.text = current_editing_entry.current_knight_info.name.ToString();
                break;
            case LineButtonMode.MODE:
                current_editing_entry.current_mode = (SyncMode)value;
                // current_editing_entry.mode.ButtonText.text = current_editing_entry.current_mode.ToString();
                break;
            case LineButtonMode.HORNET_VALUE:
                current_editing_entry.current_hornet_value = current_editing_entry.current_hornet_info.Display2Store(value, index);
                // current_editing_entry.hornet_value.ButtonText.text = value.ToString();
                break;
            case LineButtonMode.KNIGHT_VALUE:
                current_editing_entry.current_knight_value = current_editing_entry.current_knight_info.Display2Store(value, index);
                // current_editing_entry.knight_value.ButtonText.text = value.ToString();
                break;
        }
        current_editing_entry.error = false;
        current_editing_entry.CheckLock();
        current_editing_entry.update_action?.Invoke();
        OptionsBlock.Visibility.VisibleSelf = false;
        UnSelectLine();
        return;
    }
    static void SelectLine(EntryLine line)
    {
        if (current_editing_entry != null)
        {
            UnSelectLine();
        }
        current_editing_entry = line;
        line.index.State = ElementState.TRUE;
        line.index.Interactable = false;
        line.CheckLock();
    }
    internal static void UnSelectLine()
    {
        if (current_editing_entry == null) return;
        if (current_editing_entry.parentgroup.Contains(OptionsBlock))
        {
            current_editing_entry.parentgroup.Remove(OptionsBlock);
            OptionsBlock.Visibility.VisibleSelf = false;
        }
        current_editing_entry.index.State = ElementState.DEFAULT;
        current_editing_entry.index.Interactable = true;
        current_selected_button = null;
        current_editing_entry = null;
    }
    internal void UnFreeze()
    {
        freeze = true;
        index.Interactable = true;
        hornet.Interactable = true;
        hornet_value.Interactable = true;
        mode.Interactable = true;
        knight.Interactable = true;
        knight_value.Interactable = true;
        add.Interactable = true;
        delete.Interactable = true;
        CheckLock();
    }
    internal void Freeze()
    {
        freeze = false;
        index.Interactable = false;
        hornet.Interactable = false;
        hornet_value.Interactable = false;
        mode.Interactable = false;
        knight.Interactable = false;
        knight_value.Interactable = false;
        add.Interactable = false;
        delete.Interactable = false;
    }
    public EntryLine()
    {
        line = new(8);
        line.HorizontalSpacing = SpacingConstants.HSPACE_MEDIUM * 4 / 11;
        line.VerticalSpacing = SpacingConstants.VSPACE_SMALL;
        index = new("index");
        line.Add(index);
        SetStableTextSize(index.ButtonText, font_size);
        index.OnSubmit = () => SelectLine(this);

        hornet = new("hornet");
        SetStableTextSize(hornet.ButtonText, font_size);
        line.Add(hornet);
        hornet.OnSubmit = () => SelectLineButton(hornet, this, LineButtonMode.HORNET_VARIABLE);
        // hornet.MenuButton.image.color = new Color(0.5f, 0.8f, 1f);

        hornet_value = new("value");
        SetStableTextSize(hornet_value.ButtonText, font_size);
        line.Add(hornet_value);
        hornet_value.OnSubmit = () => SelectLineButton(hornet_value, this, LineButtonMode.HORNET_VALUE);
        // hornet_value.MenuButton.image.color = new Color(0.5f, 0.8f, 1f);

        mode = new("mode");
        SetStableTextSize(mode.ButtonText, font_size);
        line.Add(mode);
        mode.OnSubmit = () => SelectLineButton(mode, this, LineButtonMode.MODE);
        // mode.MenuButton.image.color = new Color(0.5f, 0.8f, 1f);

        knight = new("knight");
        SetStableTextSize(knight.ButtonText, font_size);
        line.Add(knight);
        knight.OnSubmit = () => SelectLineButton(knight, this, LineButtonMode.KNIGHT_VARIABLE);
        // knight.MenuButton.image.color = new Color(0.5f, 0.8f, 1f);

        knight_value = new("value");
        SetStableTextSize(knight_value.ButtonText, font_size);
        line.Add(knight_value);
        knight_value.OnSubmit = () => SelectLineButton(knight_value, this, LineButtonMode.KNIGHT_VALUE);
        // knight_value.MenuButton.image.color = new Color(0.5f, 0.8f, 1f);


        add = new(LangKey.MM_ADD.Localize());
        SetStableTextSize(add.ButtonText, font_size);
        add.SetMainColor(Color.green);
        add.ApplyDefaultColors = false;
        add.OnSubmit = () => Spawn(scroll, scroll.entities.IndexOf(parentgroup) + 1, current_lines, up, down, update_action);
        delete = new(LangKey.MM_DELETE.Localize());
        delete.ApplyDefaultColors = false;
        SetStableTextSize(delete.ButtonText, font_size);
        delete.SetMainColor(Color.red);
        delete.OnSubmit = () => Recycle();
        line.Add(add);
        line.Add(delete);
        line.Visibility.VisibleSelf = false;
        parentgroup = new();
        parentgroup.Add(line, Vector2.zero);
        available_lines.Add(this);
    }
    internal bool Validate()
    {
        bool error = true;
        List<TextButton> button_list = [hornet, hornet_value, mode, knight, knight_value];
        List<TextButton> error_buttons = new();
        switch (current_mode)
        {
            case SyncMode.EQUAL:
                if (current_hornet_info != null && current_knight_info != null && current_hornet_info.type == current_knight_info.type)
                {
                    error = false;
                }
                else
                {
                    error_buttons.Add(hornet);
                    error_buttons.Add(knight);
                }
                break;
            case SyncMode.CONTRIBUTE:
                if (current_hornet_info != null && current_hornet_value != null && current_knight_info != null)
                {
                    error = false;
                }
                else
                {
                    if (current_hornet_info == null) error_buttons.Add(hornet);
                    if (current_knight_info == null) error_buttons.Add(knight);
                    if (current_hornet_value == null) error_buttons.Add(hornet_value);
                }
                break;
            case SyncMode.SET:
                if (current_knight_info == null || current_knight_value == null)
                {
                    error = true;
                    if (current_knight_info == null) error_buttons.Add(knight);
                    if (current_knight_value == null) error_buttons.Add(knight_value);
                    break;
                }
                if ((current_hornet_info == null) ^ (current_hornet_value == null))
                {
                    error_buttons.Add(hornet);
                    error_buttons.Add(hornet_value);
                    error = true;
                    break;
                }
                error = false;
                break;
            default:
                error_buttons.Add(mode);
                break;
        }
        foreach (var button in button_list)
        {
            if (error && error_buttons.Contains(button))
            {
                button.SetMainColor(Color.red);
            }
            else
            {
                button.MaybeApplyDefaultColors();
            }
        }
        this.error = error;
        return !error;
    }
    public static EntryLine Spawn(ScrollContent scroll, List<EntryLine> lines, TextButton up, TextButton down, Action updateAction)
    {
        return EntryLine.Spawn(scroll, scroll.AllEntities().Count(), lines, up, down, updateAction);
    }
    public static EntryLine Spawn(ScrollContent scroll, int index, List<EntryLine> lines, TextButton up, TextButton down, Action updateAction)
    {
        if (index < 0 || index > scroll.AllEntities().Count())
        {
            $"Spawn EntryLine at {index} Error".LogWarning();
            return null;
        }
        if (available_lines.Count == 0)
        {
            new EntryLine();
        }
        var entry_line = available_lines[0];
        available_lines.RemoveAt(0);
        entry_line.spawned = true;
        entry_line.line.Visibility.VisibleSelf = true;
        entry_line.up = up;
        entry_line.down = down;
        entry_line.update_action = updateAction;
        entry_line.scroll = scroll;
        entry_line.current_lines = lines;
        scroll.Insert(index, entry_line.parentgroup, (_) => entry_line.GetHeight(), (_) => SpacingConstants.VSPACE_SMALL / 2);
        lines.Insert(index, entry_line);
        UpdateFrom(scroll, lines, index);
        entry_line.current_hornet_info = null;
        entry_line.current_hornet_value = null;
        entry_line.current_mode = null;
        entry_line.current_knight_info = null;
        entry_line.current_knight_value = null;
        entry_line.CheckLock();
        return entry_line;
    }
    public void Recycle()
    {
        this.spawned = false;
        this.line.Visibility.VisibleSelf = false;
        UnSelectLine();
        scroll.Remove(this.parentgroup);
        current_lines.Remove(this);
        available_lines.Add(this);
        UpdateFrom(scroll, current_lines, index_int - 1);
    }
    internal void Init(SyncBaseInfo baseInfo)
    {
        current_hornet_info = AcceptableHornetVariables.Find((info) => info.id == baseInfo.hdPath);
        current_knight_info = AcceptableKnightVariables.Find((info) => info.id == baseInfo.kdPath);
        current_mode = Enum.TryParse(typeof(SyncMode), baseInfo.operation, out object mode) ? (SyncMode)mode : null;
        current_hornet_value = current_hornet_info == null ? null : baseInfo.hdValue;
        current_knight_value = current_knight_info == null ? null : baseInfo.kdValue;
        CheckLock();
    }
    internal SyncBaseInfo SaveToBaseInfo()
    {
        if (!Validate())
        {
            $"Validate ERROR, Please Check".LogWarning();
            return null;
        }
        string hd_path = current_hornet_info == null ? null : current_hornet_info.id;
        string kd_path = current_knight_info == null ? null : current_knight_info.id;
        object hd_value = current_hornet_info == null ? null : current_hornet_value;
        object kd_value = current_knight_info == null ? null : current_knight_value;
        return new(hd_path, hd_value, kd_path, kd_value, current_mode.Value);
    }
    internal static void UpdateFrom(ScrollContent scroll, List<EntryLine> lines, int index)
    {
        // $"Update from {index} to {scroll.AllEntities().Count()}".LogDebug();
        for (int i = 0; i < scroll.AllEntities().Count(); i++)
        {
            if (i < index) continue;
            (lines[i] as EntryLine).UpdateIndex(i + 1);
        }
    }
    internal void UpdateIndex(int index)
    {
        this.index.ButtonText.text = index.ToString();
        this.index_int = index;
    }
    internal void Update()
    {

    }
    internal float GetHeight()
    {
        if (!(current_editing_entry == this && OptionsBlock.Visibility.VisibleSelf))
        {
            return SpacingConstants.VSPACE_SMALL;
        }
        else
        {
            return SpacingConstants.VSPACE_SMALL + Mathf.CeilToInt((float)OptionsBlock.AllEntities().Count((m) => m.Visibility.VisibleSelf) / (float)OptionsBlock.Columns) * OptionsBlock.VerticalSpacing;
        }
    }
    internal void LockButton(LineButtonMode mode)
    {
        switch (mode)
        {
            case LineButtonMode.HORNET_VARIABLE:
                hornet.Interactable = false;
                current_hornet_info = null;
                hornet.State = ElementState.FALSE;
                break;
            case LineButtonMode.HORNET_VALUE:
                hornet_value.Interactable = false;
                current_hornet_value = null;
                hornet_value.State = ElementState.FALSE;
                break;
            case LineButtonMode.MODE:
                this.mode.Interactable = false;
                current_mode = null;
                this.mode.State = ElementState.FALSE;
                break;
            case LineButtonMode.KNIGHT_VARIABLE:
                knight.Interactable = false;
                current_knight_info = null;
                knight.State = ElementState.FALSE;
                break;
            case LineButtonMode.KNIGHT_VALUE:
                knight_value.Interactable = false;
                current_knight_value = null;
                knight_value.State = ElementState.FALSE;
                break;
            default:
                break;
        }
    }
    internal void UpdateButtonText()
    {
        if (current_hornet_info == null)
        {
            hornet.ButtonText.text = "hornet";
            hornet.State = ElementState.DEFAULT;
        }
        else
        {
            hornet.ButtonText.text = current_hornet_info.name;
            hornet.State = ElementState.TRUE;
        }
        if (current_hornet_value == null)
        {
            hornet_value.ButtonText.text = "value";
            hornet_value.State = ElementState.DEFAULT;
        }
        else
        {
            hornet_value.ButtonText.text = current_hornet_info.Store2Display(current_hornet_value, null).ToString();
            hornet_value.State = ElementState.TRUE;
        }

        if (current_mode == null)
        {
            mode.ButtonText.text = "mode";
            mode.State = ElementState.DEFAULT;
        }
        else
        {
            mode.ButtonText.text = ((LangKey)Enum.Parse(typeof(LangKey), "SYNC_MODE_" + current_mode.ToString())).Localize();
            mode.State = ElementState.TRUE;
        }

        if (current_knight_info == null)
        {
            knight.ButtonText.text = "knight";
            knight.State = ElementState.DEFAULT;
        }
        else
        {
            knight.ButtonText.text = current_knight_info.name;
            knight.State = ElementState.TRUE;
        }
        if (current_knight_value == null)
        {
            knight_value.ButtonText.text = "value";
            knight_value.State = ElementState.DEFAULT;
        }
        else
        {
            knight_value.ButtonText.text = current_knight_info.Store2Display(current_knight_value, null).ToString();
            knight_value.State = ElementState.TRUE;
        }
        if (!error)
        {
            hornet.MaybeApplyDefaultColors();
            hornet_value.MaybeApplyDefaultColors();
            mode.MaybeApplyDefaultColors();
            knight.MaybeApplyDefaultColors();
            knight_value.MaybeApplyDefaultColors();
        }


    }
    internal void UnLockButton(LineButtonMode mode)
    {
        switch (mode)
        {
            case LineButtonMode.HORNET_VARIABLE:
                hornet.Interactable = true;
                hornet.State = ElementState.DEFAULT;
                break;
            case LineButtonMode.HORNET_VALUE:
                hornet_value.Interactable = true;
                hornet_value.State = ElementState.DEFAULT;
                break;
            case LineButtonMode.MODE:
                this.mode.Interactable = true;
                this.mode.State = ElementState.DEFAULT;
                break;
            case LineButtonMode.KNIGHT_VARIABLE:
                knight.Interactable = true;
                knight.State = ElementState.DEFAULT;
                break;
            case LineButtonMode.KNIGHT_VALUE:
                knight_value.Interactable = true;
                knight_value.State = ElementState.DEFAULT;
                break;
            default:
                break;
        }
    }
    internal void CheckLock()
    {
        UnLockButton(LineButtonMode.MODE);
        switch (current_mode)
        {
            case SyncMode.CONTRIBUTE:
                UnLockButton(LineButtonMode.HORNET_VARIABLE);
                UnLockButton(LineButtonMode.HORNET_VALUE);
                UnLockButton(LineButtonMode.KNIGHT_VARIABLE);
                LockButton(LineButtonMode.KNIGHT_VALUE);
                break;
            case SyncMode.EQUAL:
                UnLockButton(LineButtonMode.HORNET_VARIABLE);
                LockButton(LineButtonMode.HORNET_VALUE);
                UnLockButton(LineButtonMode.KNIGHT_VARIABLE);
                LockButton(LineButtonMode.KNIGHT_VALUE);
                break;
            case SyncMode.SET:
            case null:
            default:
                UnLockButton(LineButtonMode.HORNET_VARIABLE);
                UnLockButton(LineButtonMode.HORNET_VALUE);
                UnLockButton(LineButtonMode.KNIGHT_VARIABLE);
                UnLockButton(LineButtonMode.KNIGHT_VALUE);
                break;
        }
        if (current_hornet_info == null)
        {
            LockButton(LineButtonMode.HORNET_VALUE);
        }
        if (current_knight_info == null)
        {
            LockButton(LineButtonMode.KNIGHT_VALUE);
        }
        this.up.Interactable = false;
        this.down.Interactable = false;
        if (current_editing_entry != null && current_editing_entry == this)
        {
            if (current_editing_entry.index_int > 1)
            {
                current_editing_entry.up.Interactable = true;
            }
            if (current_editing_entry.index_int < current_editing_entry.current_lines.Count)
            {
                current_editing_entry.down.Interactable = true;
            }
        }
        UpdateButtonText();
    }
}

enum SlotConfigState
{
    OK,
    FAIL,
    MODDED
}
[RequiresMod(ModMenuPlugin.Id)]
public class WithModMenu : ICompatibility
{
    public string ModId => ModMenuPlugin.Id;

    public string ModName => "ModMenu";

    static MenuElement GenerateLocalizedBoolElement(ConfigEntryBase entry, LocalisedString? name = null, LocalisedString? desc = null, LocalisedString? false_string = null, LocalisedString? true_string = null)
    {
        if (entry is not ConfigEntry<bool> bool_entry)
        {
            return null;
        }
        List<(bool, string)> options = [
            (false,false_string??MoreLanguge.Off),
            (true,true_string??MoreLanguge.On)
        ];
        ChoiceElement<bool> choiceElement = new ChoiceElement<bool>(name ?? bool_entry.LabelName(), ChoiceModels.ForNamedValues(options), desc ?? bool_entry.DescriptionLine());
        choiceElement.SynchronizeWith(bool_entry);
        return choiceElement;
    }
    static MenuElement GenerateLocalizedFloatElement(ConfigEntryBase entry, bool use_slider, LocalisedString? name = null, float? min = null, float? max = null, int? tick = null)
    {
        if (entry is not ConfigEntry<float> float_entry)
        {
            return null;
        }
        if (use_slider && (min != null && max != null))
        {
            var model = SliderModels.ForFloats(min.Value, max.Value, tick ?? 10);
            SliderElement<float> slider = new(name ?? entry.LabelName(), model);
            slider.SynchronizeWith<float>(float_entry);
            return slider;
        }
        ParserTextModel<float> textmodel = ((entry.Description.AcceptableValues is AcceptableValueRange<float> acceptableValueRange) ? TextModels.ForFloats(acceptableValueRange.MinValue, acceptableValueRange.MaxValue) : TextModels.ForFloats());
        TextInput<float> textInput = new TextInput<float>(entry.LabelName(), textmodel, entry.DescriptionLine());
        textInput.SynchronizeWith(float_entry);
        return textInput;
    }
    static MenuElement GenerateLocalizedKeyCodeElement(ConfigEntryBase entry, LocalisedString? name = null)
    {
        if (entry is not ConfigEntry<KeyCode> keycode_entry)
        {
            return null;
        }
        var e = new KeyBindElement(name ?? entry.LabelName());
        e.SynchronizeWith(keycode_entry);
        return e;
    }

    static float CalVerticalHeight(VerticalGroup group)
    {
        return group.entities.Count * SpacingConstants.VSPACE_MEDIUM;
    }
    static float CalGridHeight(GridGroup group)
    {
        return group.Rows * group.VerticalSpacing;
    }
    public void Init()
    {
        Registry.AddModMenu("KnightInSilksong", GenerateMenu);
        KP.Instance.self_hormony.PatchAll(typeof(WithModMenu));
    }
    [HarmonyPrefix]
    [HarmonyPatch(typeof(MenuElement), nameof(MenuElement.ClearGameObjectParent), MethodType.Normal)]
    public static bool MenuElement_ClearGameObjectParent_Prefix(MenuElement __instance)
    {
        __instance.Container.transform.SetParent(null, false);
        return false;
    }
    public void Update()
    {

    }
    SelectableElement GenerateMenu()
    {
        ConfigEntryFactory factory = new();
        SimpleMenuScreen main_screen = new("KnightInSilksong");
        main_screen.Add(GenerateLocalizedBoolElement(KP.allowLog, LangKey.MM_ALLOW_LOG.Localize()));
        main_screen.Add(GenerateLocalizedKeyCodeElement(KP.toggleButton, LangKey.MM_TOGGLE_BUTTON.Localize()));
        main_screen.Add(GenerateLocalizedBoolElement(KP.apply_damage_scaling, LangKey.MM_APPLY_DAMAGE_SCALING.Localize()));
        main_screen.Add(GenerateLocalizedBoolElement(KP.default_sync, LangKey.MM_DEFAULT_SYNC.Localize()));
        main_screen.Add(GenerateLocalizedFloatElement(KP.knight_scaleX, true, LangKey.MM_KNIGHT_SCALE_X.Localize(), 1, 3, 11));
        main_screen.Add(GenerateLocalizedFloatElement(KP.knight_scaleY, true, LangKey.MM_KNIGHT_SCALE_Y.Localize(), 1, 3, 11));

        PaginatedMenuScreen slot_screen = new("KIS-Save");
        for (int i = 1; i <= 4; i++)
        {

            var sd = new SlotData(i);
            float scoll_height = 600f;
            float scoll_width = 2200f;
            FreeGroup group = new();
            ScrollContent outside_scoll = new(width: 2400, color: new Color(0, 1, 0, 0));
            ScrollContent scollList = new(scoll_height, scoll_width, new Color(0.1f, 0.1f, 0.1f, 0.5f));
            group.Add(outside_scoll, Vector2.zero - SpacingConstants.TOP_CENTER_ANCHOR);
            outside_scoll.Add(GenerateLocalizedBoolElement(sd.sync, LangKey.MM_SYNC.Localize()), (_) => SpacingConstants.VSPACE_MEDIUM);
            GridGroup edit_line = new(5);
            edit_line.HorizontalSpacing = SpacingConstants.HSPACE_MEDIUM * 2 / 5;
            TextButton reload = new(LangKey.MM_RELOAD.Localize());
            EntryLine.SetStableTextSize(reload.ButtonText, 30);
            TextButton default_button = new(LangKey.MM_DEFAULT.Localize());
            EntryLine.SetStableTextSize(default_button.ButtonText, 30);
            TextButton edit = new(LangKey.MM_EDIT.Localize());
            edit.State = ElementState.FALSE;
            EntryLine.SetStableTextSize(edit.ButtonText, 68);
            TextButton add = new(LangKey.MM_ADD.Localize());
            EntryLine.SetStableTextSize(add.ButtonText, 30);
            TextButton clear = new(LangKey.MM_CLEAR.Localize());
            EntryLine.SetStableTextSize(clear.ButtonText, 30);
            edit_line.Add(reload);
            edit_line.Add(default_button);
            edit_line.Add(edit);
            edit_line.Add(add);
            edit_line.Add(clear);
            outside_scoll.Add(edit_line, (_) => SpacingConstants.VSPACE_MEDIUM);
            outside_scoll.Add(scollList, (_) => scoll_height);
            TextButton up = new("↑");
            TextButton down = new("↓");
            TextButton validate = new(LangKey.MM_VALIDATE.Localize());
            TextButton save = new(LangKey.MM_SAVE.Localize());
            TextLabel state = new(LangKey.MM_STATE.Localize());
            up.Interactable = false;
            down.Interactable = false;
            void UpdateState(SlotConfigState update_state)
            {
                switch (update_state)
                {
                    case SlotConfigState.FAIL:
                        state.Text.text = LangKey.MM_FAIL.Localize();
                        state.State = ElementState.INVALID;
                        validate.SetMainColor(Color.red);
                        break;
                    case SlotConfigState.OK:
                        state.Text.text = LangKey.MM_OK.Localize();
                        state.State = ElementState.DEFAULT;
                        validate.SetMainColor(Color.green);
                        state.SetMainColor(Color.green);
                        break;
                    case SlotConfigState.MODDED:
                        state.Text.text = LangKey.MM_MODDED.Localize();
                        validate.MaybeApplyDefaultColors();
                        state.State = ElementState.TRUE;
                        break;
                    default:
                        break;
                }
                return;
            }


            List<EntryLine> entry_lines = new();
            sd.LoadSyncConfig();
            foreach (var base_info in sd.baseInfos)
            {
                var entry_line = EntryLine.Spawn(scollList, entry_lines, up, down, () => UpdateState(SlotConfigState.MODDED));
                entry_line.Init(base_info);
                entry_line.CheckLock();
            }
            reload.OnSubmit = () =>
            {
                EntryLine.UnSelectLine();
                for (int i = entry_lines.Count; i > 0; i--)
                {
                    entry_lines[i - 1].Recycle();
                }
                sd.LoadSyncConfig();
                foreach (var base_info in sd.baseInfos)
                {
                    var entry_line = EntryLine.Spawn(scollList, entry_lines, up, down, () => UpdateState(SlotConfigState.MODDED));
                    entry_line.Init(base_info);
                    entry_line.CheckLock();
                }
                UpdateState(SlotConfigState.MODDED);
            };
            default_button.OnSubmit = () =>
            {
                EntryLine.UnSelectLine();
                for (int i = entry_lines.Count; i > 0; i--)
                {
                    entry_lines[i - 1].Recycle();
                }
                sd.baseInfos = SyncManager.DefaultConfig();
                foreach (var base_info in sd.baseInfos)
                {
                    var entry_line = EntryLine.Spawn(scollList, entry_lines, up, down, () => UpdateState(SlotConfigState.MODDED));
                    entry_line.Init(base_info);
                    entry_line.CheckLock();
                }
                UpdateState(SlotConfigState.MODDED);
            };
            add.OnSubmit = () =>
            {
                EntryLine.UnSelectLine();
                var entry_line = EntryLine.Spawn(scollList, entry_lines, up, down, () => UpdateState(SlotConfigState.MODDED));
                entry_line.CheckLock();
                UpdateState(SlotConfigState.MODDED);
            };
            clear.OnSubmit = () =>
            {
                EntryLine.UnSelectLine();
                for (int i = entry_lines.Count; i > 0; i--)
                {
                    entry_lines[i - 1].Recycle();
                }
                UpdateState(SlotConfigState.MODDED);
            };
            GridGroup op_line = new(5);
            up.OnSubmit = () =>
            {
                var index = EntryLine.current_editing_entry.index_int;
                var entryline = EntryLine.current_editing_entry;
                entry_lines.Remove(entryline);
                scollList.Remove(entryline.parentgroup);
                entry_lines.Insert(index - 2, entryline);
                scollList.Insert(index - 2, entryline.parentgroup, (_) => entryline.GetHeight(), (_) => SpacingConstants.VSPACE_SMALL / 2);
                EntryLine.UpdateFrom(scollList, entry_lines, index - 2);
                entryline.CheckLock();
            };
            down.OnSubmit = () =>
            {
                var index = EntryLine.current_editing_entry.index_int;
                var entryline = EntryLine.current_editing_entry;
                entry_lines.Remove(entryline);
                scollList.Remove(entryline.parentgroup);
                entry_lines.Insert(index, entryline);
                scollList.Insert(index, entryline.parentgroup, (_) => entryline.GetHeight(), (_) => SpacingConstants.VSPACE_SMALL / 2);
                EntryLine.UpdateFrom(scollList, entry_lines, index - 1);
                entryline.CheckLock();
            };
            validate.OnSubmit = () =>
            {
                bool res = true;
                foreach (var line in entry_lines)
                {
                    res &= line.Validate();
                }
                if (res)
                {
                    validate.SetMainColor(Color.green);
                }
                else
                {
                    validate.SetMainColor(Color.red);
                }
            }
                ;
            save.OnSubmit = () =>
            {
                bool res = true;
                foreach (var line in entry_lines)
                {
                    res &= line.Validate();
                }
                if (res)
                {
                    UpdateState(SlotConfigState.OK);
                    sd.baseInfos = new();
                    foreach (var line in entry_lines)
                    {
                        sd.baseInfos.Add(line.SaveToBaseInfo());
                    }
                    sd.SaveSyncConfig();
                }
                else
                {
                    UpdateState(SlotConfigState.FAIL);
                }
            };
            void UpdateEdit(bool on)
            {
                if (GameManager.instance.IsGameplayScene() && on) return;
                edit.State = on ? ElementState.TRUE : ElementState.FALSE;
                reload.Interactable = on;
                reload.State = on ? ElementState.DEFAULT : ElementState.FALSE;
                default_button.Interactable = on;
                default_button.State = on ? ElementState.DEFAULT : ElementState.FALSE;
                add.Interactable = on;
                add.State = on ? ElementState.DEFAULT : ElementState.FALSE;
                clear.Interactable = on;
                clear.State = on ? ElementState.DEFAULT : ElementState.FALSE;
                up.Interactable = on;
                up.State = on ? ElementState.DEFAULT : ElementState.FALSE;
                down.Interactable = on;
                down.State = on ? ElementState.DEFAULT : ElementState.FALSE;
                EntryLine.UnSelectLine();
                foreach (var entry_line in entry_lines)
                {
                    if (on)
                    {
                        entry_line.UnFreeze();
                    }
                    else
                    {
                        entry_line.Freeze();
                    }

                }
            }
            edit.OnSubmit = () =>
            {

                if (edit.State == ElementState.TRUE)
                {
                    UpdateEdit(false);
                }
                else
                {
                    UpdateEdit(true);
                }
            };

            op_line.HorizontalSpacing = SpacingConstants.HSPACE_MEDIUM * 2 / 5;
            op_line.Add(up);
            op_line.Add(down);
            op_line.Add(validate);
            op_line.AddAt(0, 3, save);
            op_line.AddAt(0, 4, state);

            outside_scoll.Add(op_line);
            slot_screen.AddPage(group);
            UpdateEdit(false);
            slot_screen.pageNumberModel.OnValueChanged += (value) =>
            {
                EntryLine.UnSelectLine();
                up.Interactable = false;
                down.Interactable = false;
            };
            slot_screen.OnGoBack -= () => UpdateEdit(false);
            slot_screen.OnGoBack += () => UpdateEdit(false);
        }



        TextButton slot_button = new(LangKey.MM_SAVE_OPTIONS.Localize())
        {
            OnSubmit = () =>
            {
                MenuScreenNavigation.Show(slot_screen);
            }
        };
        main_screen.Add(slot_button);

        TextButton menu_button = new("KnightInSilksong");
        menu_button.OnSubmit += () =>
                    {
                        MenuScreenNavigation.Show(main_screen);
                    };
        return menu_button;
    }

}


