using System.Diagnostics.CodeAnalysis;
using AsmResolver.IO;
using BepInEx.Configuration;
using HutongGames.PlayMaker.Actions;
using KIS.Compatibility;
using PolyAndCode.UI;
using Silksong.ModMenu;
using Silksong.ModMenu.Elements;
using Silksong.ModMenu.Internal;
using Silksong.ModMenu.Models;
using Silksong.ModMenu.Plugin;
using Silksong.ModMenu.Screens;
using TeamCherry.Localization;
using UnityEngine.UI;
using KP = KIS.KnightInSilksong;

namespace KIS.Compatibility;

internal class ModMenuIgnoreAttribute : System.Attribute { }



public class ScollContent : AbstractGroup
{
    public readonly IndexedList<(IMenuEntity, Func<IMenuEntity, float>, Func<IMenuEntity, float>)> entities_with_height = new();
    public readonly IndexedList<IMenuEntity> entities = new IndexedList<IMenuEntity>();
    public static GameObject GetPrefab()
    {
        var content = UIManager.instance.UICanvas.transform.Find("AchievementsMenuScreen/Content").gameObject;
        var new_go = GameObject.Instantiate(content);
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
        return new_go;
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

    static void SetRectTransformHeight(RectTransform rect, float height)
    {
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, height);
    }
    static void SetRectTransformWidth(RectTransform rect, float width)
    {
        rect.sizeDelta = new Vector2(width, rect.sizeDelta.y);
    }

    public ScollContent(float height = 906f, float width = 2000f, Color? color = null) : base()
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
            (false,false_string??new LocalisedString("sheet","key")),
            (true,true_string??new LocalisedString("sheet","key"))
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
    }

    public void Update()
    {

    }
    SelectableElement GenerateMenu()
    {
        ConfigEntryFactory factory = new();
        SimpleMenuScreen main_screen = new("KnightInSilksong");
        main_screen.Add(GenerateLocalizedBoolElement(KP.allowLog));
        main_screen.Add(GenerateLocalizedKeyCodeElement(KP.toggleButton));
        main_screen.Add(GenerateLocalizedBoolElement(KP.apply_damage_scaling));
        main_screen.Add(GenerateLocalizedBoolElement(KP.default_sync));
        main_screen.Add(GenerateLocalizedFloatElement(KP.knight_scaleX, true, new(null, KP.knight_scaleX.LabelName()), 1, 3, 11));
        main_screen.Add(GenerateLocalizedFloatElement(KP.knight_scaleY, true, new(null, KP.knight_scaleY.LabelName()), 1, 3, 11));

        PaginatedMenuScreen slot_screen = new("KIS-Slot");
        for (int i = 1; i <= 4; i++)
        {

            var sd = new SlotData(i);
            float scoll_height = 600f;
            float scoll_width = 1600f;
            FreeGroup group = new();
            ScollContent outside_scoll = new(color: new Color(0, 1, 0, 0));
            ScollContent scollList = new(scoll_height, scoll_width, new Color(0.1f, 0.1f, 0.1f, 0.5f));
            group.Add(outside_scoll, Vector2.zero - SpacingConstants.TOP_CENTER_ANCHOR);
            outside_scoll.Add(GenerateLocalizedBoolElement(sd.sync), (_) => SpacingConstants.VSPACE_MEDIUM);
            GridGroup edit_line = new(5);
            edit_line.HorizontalSpacing = SpacingConstants.HSPACE_MEDIUM * 2 / 5;
            edit_line.Add(new TextButton("Reset"));
            edit_line.Add(new TextButton("Clear"));
            edit_line.Add(new TextButton("EDIT"));
            edit_line.AddAt(0, 4, new TextButton("Save"));
            outside_scoll.Add(edit_line, (_) => SpacingConstants.VSPACE_MEDIUM);
            if (i == 4)
            {
                outside_scoll.Add(new TextButton("test"), (_) => SpacingConstants.VSPACE_MEDIUM);
                outside_scoll.Add(new TextButton("test"), (_) => SpacingConstants.VSPACE_MEDIUM);
                outside_scoll.Add(new TextButton("test"), (_) => SpacingConstants.VSPACE_MEDIUM);
                outside_scoll.Add(new TextButton("test"), (_) => SpacingConstants.VSPACE_MEDIUM);
            }
            outside_scoll.Add(scollList, (_) => scoll_height);

            VerticalGroup group1 = new();
            scollList.Add(group1, (e) => ((VerticalGroup)e).entities.Count * SpacingConstants.VSPACE_MEDIUM, (_) => SpacingConstants.VSPACE_MEDIUM / 2);
            for (int j = 0; j <= i * 10; j++)
            {
                // group1.Add(new TextLabel("TEST" + j));
                TextButton button = new("123456");
                button.SetFontSizes(FontSizes.Medium);
                group1.Add(button);
            }
            slot_screen.AddPage(group);
        }


        TextButton slot_button = new("Slot Options")
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


