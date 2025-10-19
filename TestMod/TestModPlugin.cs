using BepInEx;
using HarmonyLib;
using UnityEngine;
using HutongGames.PlayMaker;
using System.Reflection;
using BepInEx.Logging;
using System.Collections.Generic;
using KIS.Utils;

namespace KIS;

// TODO - adjust the plugin guid as needed
[BepInAutoPlugin(id: "io.github.shownyoung.testmod")]
public partial class KnightInSilksong : BaseUnityPlugin
{
    Fsm fsm = null;
    public GameObject bundle = null;
    internal static KnightInSilksong Instance = null;
    internal static ManualLogSource logger = null;
    public AssetBundle hk = null;
    public AssetBundle mat = null;
    public GameObject knight = null;
    public GameObject hud_canvas = null;
    public static Dictionary<string, GameObject> loaded_gos = new();
    public static List<Material> loaded_mats = new();
    Knight.HeroController KnightController => Knight.HeroController.instance;
    GameObject hud_instance = null;
    public GameObject charm = null;
    public GameObject charm_instance = null;
    public GameObject fury_effect = null;
    public GameObject fury_effect_instance = null;

    internal static bool IsKnight => Instance.iskight;
    bool iskight = false;
    public static Int32 KnightDamage => 1 << 30;
    public Harmony self_hormony;
    public Action<bool> OnToggleKnight = null;

    private void Awake()
    {
        logger = Logger;
        Instance = this;
        // Put your initialization logic here
        Logger.LogInfo($"Plugin {Name} ({Id}) has loaded!");
        Logger.LogInfo($"Game version: {Application.version}");
        Logger.LogInfo($"Unity version: {Application.unityVersion}");
        hk = AssetBundle.LoadFromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("TestMod.Resources.knight"));
        var gos = hk.LoadAllAssets<GameObject>();
        foreach (var go in gos)
        {
            loaded_gos.Add(go.name, go);
        }
        knight = loaded_gos["Knight_0"];
        hud_canvas = loaded_gos["Hud Canvas"];
        charm = loaded_gos["Charms"];
        fury_effect = loaded_gos["fury_effects_v2"];
        DontDestroyOnLoad(knight);
        ModuleManager.Init();
        self_hormony = new Harmony(Id);
        foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
        {
            if (type.IsSubclassOf(typeof(GeneralPatch)) && !type.IsAbstract)
            {
                self_hormony.PatchAll(type);
            }
        }


        // SetKnight(knight);


    }
    private void ToggleKnight()
    {
        bool laststate = iskight;
        iskight = !iskight;
        if (laststate)
        {
            if (KnightController != null)
            {
                KnightController.gameObject.SetActive(false);
                hud_instance.SetActive(false);
                HeroController.instance.GetComponent<MeshRenderer>().enabled = true;
                HeroController.instance.enabled = true;
                foreach (var fsm in HeroController.instance.GetComponents<PlayMakerFSM>())
                {
                    fsm.enabled = true;
                }
                HeroController.instance.gameObject.FindGameObjectInChildren("HeroBox").SetActive(true);
                HudCanvas.instance.gameObject.SetActive(true);
            }
        }
        else
        {
            HeroController.instance.GetComponent<MeshRenderer>().enabled = false;
            HeroController.instance.enabled = false;
            foreach (var fsm in HeroController.instance.GetComponents<PlayMakerFSM>())
            {
                fsm.enabled = false;
            }
            HeroController.instance.gameObject.FindGameObjectInChildren("HeroBox").SetActive(false);
            HudCanvas.instance.gameObject.SetActive(false);
            if (KnightController == null)
            {
                knight.GetComponent<Knight.HeroController>().hardLandingEffectPrefab = HeroController.instance.hardLandingEffectPrefab;
                knight.GetComponent<Knight.HeroController>().softLandingEffectPrefab = HeroController.instance.softLandingEffectPrefab;
                GameObject.Instantiate(knight);
                KnightController.gameObject.FindGameObjectInChildren("Attacks").Find("Slash").LocateMyFSM("damages_enemy").InsertCustomAction("Send Event", () => { "Send Event".LogInfo(); }, 0);
                KnightController.gameObject.FindGameObjectInChildren("Charm Effects").LocateMyFSM("Fury").AddCustomAction("Get Ref", (fsm) =>
                {
                    fsm.GetVariable<FsmGameObject>("Fury Vignette").Value = fury_effect_instance;
                });


                hud_instance = GameObject.Instantiate(hud_canvas);
                hud_instance.transform.SetParent(HudCanvas.instance.gameObject.transform.parent, true);
                GameCameras.instance.soulOrbFSM = hud_instance.FindGameObjectInChildren("Soul Orb").LocateMyFSM("Soul Orb Control");
                GameCameras.instance.soulVesselFSM = hud_instance.FindGameObjectInChildren("Soul Orb").FindGameObjectInChildren("Vessels").LocateMyFSM("Update Vessels");
                GameManager.instance.soulOrb_fsm = GameCameras.instance.soulOrbFSM;
                GameManager.instance.soulVessel_fsm = GameCameras.instance.soulVesselFSM;

                charm_instance = GameObject.Instantiate(charm);
                charm_instance.transform.SetParent(HudCanvas.instance.transform.parent.parent.parent.Find("Inventory").Find("Tools"), false);
                charm_instance.transform.position = new Vector3(-4.05f, 7.55f, 2.3f);

                fury_effect_instance = GameObject.Instantiate(fury_effect);
                DontDestroyOnLoad(fury_effect_instance);



            }
            else
            {
                KnightController.gameObject.SetActive(true);
                hud_instance.SetActive(true);
            }
        }
        OnToggleKnight?.Invoke(iskight);

    }


    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.F5))
        {
            ToggleKnight();
        }
    }

}