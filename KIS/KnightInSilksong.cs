using BepInEx;
using HarmonyLib;
using UnityEngine;
using HutongGames.PlayMaker;
using System.Reflection;
using BepInEx.Logging;
using System.Collections.Generic;
using KIS.Utils;
using HutongGames.PlayMaker.Actions;
using UnityEngine.Audio;
using BepInEx.Configuration;

namespace KIS;

// TODO - adjust the plugin guid as needed
[BepInAutoPlugin(id: "io.github.shownyoung.knightinsilksong")]
[BepInDependency("org.silksong-modding.fsmutil")]
public partial class KnightInSilksong : BaseUnityPlugin
{
    internal static KnightInSilksong Instance = null;
    internal static ManualLogSource logger = null;
    internal static bool return_to_main_menu = false;
    public AssetBundle hk = null;
    public GameObject knight = null;
    public GameObject hud_canvas = null;
    public static Dictionary<string, GameObject> loaded_gos = new();
    public static List<Material> loaded_mats = new();
    Knight.HeroController KnightController => Knight.HeroController.instance;
    public GameObject hud_instance = null;
    public GameObject charm = null;
    public GameObject charm_instance = null;
    public GameObject fury_effect = null;
    public GameObject fury_effect_instance = null;
    public static ConfigEntry<bool> allowLog;
    public static ConfigEntry<KeyCode> toggleButton;
    public static ConfigEntry<bool> apply_damage_scaling;

    internal static bool IsKnight => Instance.iskight;
    bool iskight = false;
    public static Int32 KnightDamage => 1 << 30;
    public const int HazardType_NORESPOND = 4096;
    public Harmony self_hormony;
    public Action<bool> OnToggleKnight = null;
    private static AudioMixer master = null;
    public static AudioMixer Master
    {
        get
        {
            if (master == null) master = PreProcess.Instance.share_mixer_group.First((am) => am.name == "Master");
            return master;
        }
    }

    // maybe not the best way to do this
    public static bool shouldToggleKnight = false;

    private void Awake()
    {
        logger = Logger;
        allowLog = Config.Bind<bool>("General", "AllowLog", false);
        toggleButton = Config.Bind<KeyCode>("General", "ToggleButton", KeyCode.F5);
        apply_damage_scaling = Config.Bind("Play",
                                            "ApplyDamageScaling",
                                             true,
                                             "Enable this to make knight's damage influenced by damage scaling");

        Instance = this;
        // Put your initialization logic here
        Logger.LogInfo($"Plugin {Name} ({Id}) has loaded!");
        Logger.LogInfo($"Game version: {Application.version}");
        Logger.LogInfo($"Unity version: {Application.unityVersion}");
        hk = AssetBundle.LoadFromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("KIS.Resources.knight"));
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
    internal void ToggleKnight()
    {
        if (!GameManager.instance.IsGameplayScene())
        {
            return;
        }
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
            DialogueBox._instance.hudFSM = HudCanvas.instance.gameObject.LocateMyFSM("Slide Out");
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

                InstKnight();
                fury_effect_instance = GameObject.Instantiate(fury_effect);
                DontDestroyOnLoad(fury_effect_instance);
            }
            else
            {
                KnightController.gameObject.SetActive(true);
                if (return_to_main_menu)
                {
                    Traverse.Create(KnightController).Method("SetupGameRefs").GetValue();
                    "Try SetupGameRefs".LogInfo();
                    return_to_main_menu = false;
                }
            }
            if (hud_instance == null)
            {
                InstHud(charm_instance == null);
            }
            else
            {
                hud_instance.SetActive(true);
            }
            DialogueBox._instance.hudFSM = hud_instance.LocateMyFSM("Slide Out");
        }
        OnToggleKnight?.Invoke(iskight);


        void InstHud(bool with_charm = true)
        {
            hud_instance = GameObject.Instantiate(hud_canvas);
            hud_instance.transform.SetParent(HudCanvas.instance.gameObject.transform.parent, true);
            GameCameras.instance.soulOrbFSM = hud_instance.FindGameObjectInChildren("Soul Orb").LocateMyFSM("Soul Orb Control");
            GameCameras.instance.soulVesselFSM = hud_instance.FindGameObjectInChildren("Soul Orb").FindGameObjectInChildren("Vessels").LocateMyFSM("Update Vessels");
            GameManager.instance.soulOrb_fsm = GameCameras.instance.soulOrbFSM;
            GameManager.instance.soulVessel_fsm = GameCameras.instance.soulVesselFSM;
            if (with_charm) InstCharm();

        }
        void InstCharm()
        {
            charm_instance = GameObject.Instantiate(charm);
            charm_instance.transform.SetParent(HudCanvas.instance.transform.parent.parent.parent.Find("Inventory").Find("Tools"), false);
            charm_instance.transform.position = new Vector3(-4.05f, 7.55f, 2.3f);
        }

    }

    internal void InstKnight()
    {
        knight.GetComponent<Knight.HeroController>().hardLandingEffectPrefab = HeroController.instance.hardLandingEffectPrefab;
        knight.GetComponent<Knight.HeroController>().softLandingEffectPrefab = HeroController.instance.softLandingEffectPrefab;
        GameObject.Instantiate(knight);
        // KnightController.gameObject.FindGameObjectInChildren("Attacks").Find("Slash").LocateMyFSM("damages_enemy").InsertCustomAction("Send Event", () => { "Send Event".LogInfo(); }, 0);
        KnightController.gameObject.FindGameObjectInChildren("Charm Effects").LocateMyFSM("Fury").AddCustomAction("Get Ref", (fsm) =>
        {
            fsm.GetVariable<FsmGameObject>("Fury Vignette").Value = fury_effect_instance;
        });
        KnightController.gameObject.Find("Vignette").SetActive(false);
        KnightController.gameObject.Find("white_light_donut").SetActive(false);
        KnightController.gameObject.Find("HeroLight").SetActive(false);
        KnightController.gameObject.Find("Attacks").Find("Sharp Shadow").tag = "Sharp Shadow";

        var death = KnightController.heroDeathPrefab.LocateMyFSM("Hero Death Anim");
        death.GetAction<HutongGames.PlayMaker.Actions.SetPlayerDataBool>("Limit Soul", 2).value = false;
        death.GetAction<SetBoolValue>("Limit Soul", 3).boolValue = false;
        GameObject centre = new GameObject("Centre");
        centre.transform.SetParent(KnightController.transform, false);
        centre.transform.localPosition = new Vector3(0f, 0f, 0f);
        var roar = KnightController.gameObject.AddComponent<PlayMakerFSM>();
        roar.Fsm = new Fsm(HeroController.instance.gameObject.LocateMyFSM("Roar and Wound States").Fsm);
        roar.AddTransition("Damage Check 4", "CANCEL", "Idle");
        var hitbox = KnightController.gameObject.FindGameObjectInChildren("Dream Effects").FindGameObjectInChildren("Hitbox");

        var proxy = KnightController.gameObject.LocateMyFSM("ProxyFSM");
        var parried = proxy.AddState("Parried");
        parried.AddTransition("FINISHED", "Blocker Hit");
        parried.AddCustomAction(() =>
        {
            KnightController.StartCoroutine(KnightController.StartRecoil(GlobalEnums.CollisionSide.top, false, 0));
        });
        proxy.AddTransition("Idle", "PARRIED", "Parried");

        // hitbox.LocateMyFSM("Send Event").AddAction("Send Event", new SendDreamImpact());
    }
    private void Update()
    {
        if (Input.GetKeyDown(toggleButton.Value) || shouldToggleKnight)
        {
            ToggleKnight();
            ProgressionManager.setup();

            shouldToggleKnight = false;
        }
        ProgressionManager.setProgression();

    }
    private void OnApplicationQuit()
    {
        HelperFun.SavePlayerData();
    }

}
