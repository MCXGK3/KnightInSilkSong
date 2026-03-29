using BepInEx;
using HutongGames.PlayMaker;
using BepInEx.Logging;
using KIS.Utils;
using HutongGames.PlayMaker.Actions;
using UnityEngine.Audio;
using BepInEx.Configuration;
using TeamCherry.Localization;
using Newtonsoft.Json.UnityConverters.Helpers;
using KIS.Compatibility;

namespace KIS;

[ModMenuIgnore]
[BepInDependency(PrepatcherPlugin.PrepatcherPlugin.Id)]
[BepInAutoPlugin(id: "io.github.shownyoung.knightinsilksong")]
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
    public static ConfigEntry<bool> default_sync;
    public static ConfigEntry<float> knight_scaleX;
    public static ConfigEntry<float> knight_scaleY;
    internal static bool IsKnight => Instance.iskight;
    bool iskight = false;
    public static int KnightDamage => 1 << 30;
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

    public SlotData current_data
    {
        get;
        set
        {
            if (field != value)
            {
                if (field != null)
                {
                    field.SaveSyncConfig();
                }
                field = value;
            }
        }
    } = null;

    private void Awake()
    {
        logger = Logger;
        allowLog = Config.Bind<bool>("General", "AllowLog", false);
        toggleButton = Config.Bind<KeyCode>("General", "ToggleButton", KeyCode.F5);
        apply_damage_scaling = Config.Bind("Play",
                                            "ApplyDamageScaling",
                                             true,
                                             "Enable this to make knight's damage influenced by damage scaling");
        default_sync = Config.Bind("Play",
                                "DefaultSync",
                                false,
                                "Start game under knight sync mode");
        knight_scaleX = Config.Bind("Play",
                                "KnightScaleX",
                                1f,
                                "The X scale of the knight. Switch to apply");
        knight_scaleY = Config.Bind("Play",
                                "KnightScaleY",
                                1f,
                                "The Y scale of the knight. Switch to apply");

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
        foreach (var type in Assembly.GetExecutingAssembly().GetTypesSafely())
        {
            if (type.IsSubclassOf(typeof(GeneralPatch)) && !type.IsAbstract)
            {
                self_hormony.PatchAll(type);
            }
        }
        HandlePlayerData.Init();
        // KISHelper.OnReturnToMenu += () => current_data = null;


        // SetKnight(knight);


    }
    internal void Start()
    {
        Compatibility.CompatibilityManager.CheckAndInit();
        foreach (var type in Assembly.GetExecutingAssembly().GetTypesSafely())
        {
            if (type.IsSubclassOf(typeof(StartPatch)) && !type.IsAbstract)
            {
                self_hormony.PatchAll(type);
            }
        }
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
                EnableOriHud();
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

            SyncManager.Instance.H2KSyncData();
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
                    "Try SetupGameRefs".LogDebug();
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
            DisableOriHud();
            DialogueBox._instance.hudFSM = hud_instance.LocateMyFSM("Slide Out");
        }
        KnightController.gameObject.transform.SetScale2D(new((KnightController.transform.localScale.x > 0 ? 1 : -1) * knight_scaleX.Value, knight_scaleY.Value));
        OnToggleKnight?.Invoke(iskight);


        void InstHud(bool with_charm = true)
        {
            hud_instance = GameObject.Instantiate(hud_canvas);
            hud_instance.transform.SetParent(HudCanvas.instance.gameObject.transform.parent, true);
            GameCameras.instance.soulOrbFSM = hud_instance.FindGameObjectInChildren("Soul Orb").LocateMyFSM("Soul Orb Control");
            GameCameras.instance.soulVesselFSM = hud_instance.FindGameObjectInChildren("Soul Orb").FindGameObjectInChildren("Vessels").LocateMyFSM("Update Vessels");
            GameManager.instance.soulOrb_fsm = GameCameras.instance.soulOrbFSM;
            GameManager.instance.soulVessel_fsm = GameCameras.instance.soulVesselFSM;
            var orb_full = hud_instance.FindGameObjectInChildren("Orb Full");
            orb_full.FindGameObjectInChildren("Pulse Sprite").GetComponent<SpriteRenderer>().sprite = orb_full.GetComponent<SpriteRenderer>().sprite;
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
        ModuleManager.GetInstance<PreProcess>().StopGetShader();
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
        // death.GetAction<HutongGames.PlayMaker.Actions.SetPlayerDataBool>("Limit Soul", 2).value = false;
        // death.GetAction<SetBoolValue>("Limit Soul", 3).boolValue = false;
        death.GetAction<SendMessage>("Set Shade", 1).functionCall.FunctionName = nameof(GameMap.PositionCompassAndCorpse);
        death.InsertCustomAction("Check MP", () => PreProcess.SetDeathInfo(), 0);
        death.AddCustomAction("Map Zone", (fsm) =>
        {
            if (GameManager.instance.IsMemoryScene())
            {
                fsm.SendEvent("DREAM");
            }
        });

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
        // KnightController.ClearMP();
        Knight.PlayerData.instance.SetBool(nameof(Knight.PlayerData.infiniteAirJump), WithDebugMod.infinite_jump);
        Knight.PlayerData.instance.SetInt(nameof(Knight.PlayerData.MPCharge), 0);
        Knight.PlayerData.instance.SetInt(nameof(Knight.PlayerData.MPReserve), 0);
        //why didn't tc put the needle back in its original position?
        HeroController.instance.gameObject.LocateMyFSM("Harpoon Dash").AddCustomAction("Init", (fsm) =>
        {
            fsm.GetVariable<FsmGameObject>("Needle").Value ??= HeroController.instance.gameObject.FindGameObjectInChildren("Harpoon Needle");
            fsm.GetVariable<FsmGameObject>("Thread").Value ??= HeroController.instance.gameObject.FindGameObjectInChildren("Harpoon Needle").FindGameObjectInChildren("Thread");
        });
        // hitbox.LocateMyFSM("Send Event").AddAction("Send Event", new SendDreamImpact());

    }
    internal void DisableOriHud()
    {
        Vector3 counters_position = new(-0.024f, -1.095f, 0);
        var counters = HudCanvas.instance.gameObject.LocateMyFSM("Slide Out").FsmVariables.FindFsmGameObject("Counters Parent").Value;

        HudCanvas.instance.gameObject.LocateMyFSM("Slide Out").RemoveTransition("Out", "IN");
        HudCanvas.instance.gameObject.LocateMyFSM("Slide Out").SendEvent("OUT INSTANT");
        HudCanvas.instance.gameObject.transform.SetScale2D(Vector2.one);
        HudCanvas.instance.gameObject.LocateMyFSM("Globalise").enabled = false;



        if (counters != null)
        {
            counters.transform.SetParent(hud_instance.transform);
            counters.transform.position = counters_position;
            counters.GetComponent<PositionRelativeTo>()?.previousPos = counters.transform.position;
        }


    }
    internal void EnableOriHud()
    {
        HudCanvas.instance.gameObject.LocateMyFSM("Globalise").enabled = true;
        HudCanvas.instance.gameObject.LocateMyFSM("Slide Out").AddTransition("Out", "IN", "In Wait");
        HudCanvas.instance.gameObject.LocateMyFSM("Slide Out").SendEvent("IN");

        //keep counters
        const float original_offset_y = -9.3f;
        const float disable_offset_y = -59.3f;
    }
    private void Update()
    {

        if (Input.GetKeyDown(toggleButton.Value) || shouldToggleKnight)
        {
            ToggleKnight();
            ProgressionManager.setup();

            shouldToggleKnight = false;
        }
        if (HeroController.instance != null)
        {
            ProgressionManager.setProgression();
        }
        Compatibility.CompatibilityManager.UpdateAll();


    }
    private void OnApplicationQuit()
    {
        KISHelper.OnQuitApp?.Invoke();
        current_data = null;
    }

}
