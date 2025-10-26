using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using KIS;
using KIS.Utils;
using UnityEngine.UI;

[HarmonyPatch(typeof(InventoryItemSelectableDirectional), "Awake")]
public class Patch_InventoryItemSelectableDirectional_Awake : GeneralPatch
{
    public static GameObject charmButton;
    static DoAction button_action;
    public static bool Prefix(InventoryItemSelectableDirectional __instance)
    {
        return true;
    }
    public static void Postfix(InventoryItemSelectableDirectional __instance)
    {
        if (__instance.gameObject.name == "Change Crest Button" && __instance is InventoryItemSelectableButtonEvent button)
        {
            charmButton = GameObject.Instantiate(__instance.gameObject, __instance.gameObject.transform.parent);
            charmButton.transform.SetPosition2D(-7.8865f, -5.53f);
            button.Selectables[(int)InventoryItemManager.SelectionDirection.Right] = charmButton.GetComponent<InventoryItemSelectableDirectional>();
            charmButton.GetComponent<InventoryItemSelectableDirectional>().Selectables[(int)InventoryItemManager.SelectionDirection.Left] = __instance;
            GameObject.Destroy(charmButton.FindGameObjectInChildren("Change Crest Action"));
            var parent = charmButton.FindGameObjectInChildren("Parent");
            var locked = parent.FindGameObjectInChildren("Button Icon Locked");
            var regular = parent.FindGameObjectInChildren("Button Icon Regular");
            GameObject.Destroy(locked);
            regular.name = "Charm Icon";
            regular.transform.SetScale2D(new Vector2(0.5f, 0.5f));
            regular.GetComponent<SpriteRenderer>().sprite = ModuleManager.GetInstance<PreProcess>().charm_icon;

            if (charmButton.GetComponent<InventoryItemSelectableDirectional>() is InventoryItemSelectableButtonEvent charmButtonEvent)
            {
                button_action = charmButton.AddComponent<DoAction>();
                button_action.action += ToggleCharm;
                charmButtonEvent.ButtonActivated += button_action.DoActionNow;
            }
            charmButton.SetActive(KnightInSilksong.IsKnight);
            KnightInSilksong.Instance.OnToggleKnight += (value) =>
            {
                charmButton.SetActive(value);
            };

        }
    }
    public static void ToggleCharm(DoAction doAction = null)
    {
        DoAction arg;
        if (doAction == null)
        {
            arg = button_action;
        }
        else
        {
            arg = doAction;
        }
        var button = arg.gameObject;
        InventoryItemToolManager manager = button.transform.parent.parent.GetComponent<InventoryItemToolManager>();
        bool state = KnightInSilksong.Instance.charm_instance.activeSelf;
        manager.FadeToolGroup(state);
        manager.gameObject.FindGameObjectInChildren("Crest List").SetActive(state);
        button.transform.parent.gameObject.SetActive(state);
        KnightInSilksong.Instance.charm_instance.SetActive(!state);
        if (!state)
        {
            PlayMakerFSM fsm = KnightInSilksong.Instance.charm_instance.LocateMyFSM("UI Charms");

            if (fsm.GetValidState("Inventory Closed").actions.Length == 0)
            {
                fsm.AddCustomAction("Inventory Closed", () =>
                {
                    // if (KnightInSilksong.Instance.charm_instance.activeSelf)
                    // {
                    "Try Toggle Charm from Inventory Closed".LogInfo();
                    ToggleCharm(null);
                    // }
                });
                fsm.RemoveTransition("Left", "TO LEFT");
                fsm.RemoveTransition("Right", "TO RIGHT");
                fsm.InsertCustomAction("Left", (fsm) =>
                {
                    if (fsm.GetVariable<FsmInt>("Collection Pos").value == 1) fsm.SendEvent("FINISHED");
                }, 1);
                fsm.InsertCustomAction("Right", (fsm) =>
                {
                    if (fsm.GetVariable<FsmInt>("Collection Pos").value == 40) fsm.SendEvent("FINISHED");
                }, 1);
                KnightInSilksong.Instance.charm_instance.AddComponent<TrueCharmDisable>();
                KnightInSilksong.Instance.charm_instance.transform.Find("Equipped Charms/Next Dot/Sprite Next").gameObject.SetActive(false);
                // fsm.AddGlobalTransition("DOWN", "Inventory Closed");
                // KnightInSilksong.Instance.charm_instance.transform.parent.gameObject.LocateMyFSM("Inventory Proxy").AddCustomAction("Wait for active", () =>
                // {
                //     // if (KnightInSilksong.Instance.charm_instance.activeSelf)
                //     // {
                //     //     "Try Toggle Charm from Proxy".LogInfo();
                //     //     ToggleCharm(null);
                //     // }
                // });

            }
            fsm.FsmVariables.GetFsmInt("New Charm ID").value = 1;

        }
        FSMUtility.SendEventToGameObject(KnightInSilksong.Instance.charm_instance, "UP", true);
        FSMUtility.SendEventToGameObject(KnightInSilksong.Instance.charm_instance, "ACTIVATE", true);
        manager.GetComponent<InventoryPaneInput>().enabled = false;

    }
}