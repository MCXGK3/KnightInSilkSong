using HutongGames.PlayMaker.Actions;
using KIS.Utils;
using UnityEngine.SceneManagement;

namespace KIS;

public class ProgressionManager
{
    private static GameObject smallPlatform;

    private static int oldKnightHealth = 0;
    private static int oldKnightMPCharge = 0;
    private static int knightSoulRemainder = 0;

    private static bool managedFsmChange = false;

    private static SyncManager syncManager = new();

    private static Dictionary<string, List<Vector2>> platform_positions = new()
    {
        {"under_17", [
            new(45f, 15f)
        ]},
        {"bone_east_20",[
            new(113f,19f)
        ]
        }
    };

    public static void setup()
    {
        if (smallPlatform != null)
            return;

        setupPlatform();
        SceneManager.activeSceneChanged += onActiveSceneChanged;
    }

    private async static void setupPlatform()
    {
        smallPlatform = await SceneObjectManager.loadObjectFromScene("Tut_01b", "bone_plat_03 (2)");

        smallPlatform.SetActive(false);
        smallPlatform.name = "smallPlatform";
        UnityEngine.Object.DontDestroyOnLoad(smallPlatform);
    }

    private static void placePlatform(float x, float y)
    {

        Vector2 pos = new Vector2(x, y);
        GameObject plat = UnityEngine.Object.Instantiate(smallPlatform);
        plat.transform.position = pos;
        plat.SetActive(true);
    }

    // assumes one exists in the current scene
    private static void placeBounceBloom(float x, float y, string name)
    {
        Vector2 pos = new Vector2(x, y);

        GameObject firstBloom = GameObject.Find(name);
        GameObject plat = UnityEngine.Object.Instantiate(firstBloom);
        plat.GetComponent<Transform>().position = pos;
        plat.SetActive(true);
    }

    private static void onActiveSceneChanged(Scene from, Scene to)
    {
        string scene = to.name.ToLower();
        managedFsmChange = false;
        //special fix
        switch (scene)
        {
            case "tut_01":
                patchIntroCutscenes();
                disableWeaknessCutscene();
                break;
            case "tut_03":
                disableWeaknessCutscene();
                break;
            case "bonetown":
                PlayerData.instance.churchKeeperIntro = true;
                disableWeaknessCutscene();
                break;
            case "library_10":
                movePsalmCylinderDown();
                break;
            case "hang_01":
                moveHang01RingDown();
                break;
            case "shellwood_03":
                placeBounceBloom(10f, 21.5f, "Shellwood Bounce Bloom");
                break;
            case "shellwood_13":
                if (!PlayerData.instance.hasWalljump)
                {
                    AddNonSlider("Chunk 0 2");
                }
                break;
        }
        //place platforms
        if (platform_positions.TryGetValue(scene, out var positions))
        {
            foreach (var pos in positions)
            {
                placePlatform(pos.x, pos.y);
            }
        }

    }

    private static void AddNonSlider(string goname)
    {
        GameObject go = GameObject.Find(goname);
        go.GetAddComponent<NonSlider>();
    }

    public static void setProgression()
    {
        PlayerData hData = PlayerData.instance;
        Knight.PlayerData kData = Knight.PlayerData.instance;
        // syncManager.H2KSyncData();
        // movement
        /*kData.hasDash = hData.hasDash;
        kData.canDash = hData.hasDash;
        kData.canShadowDash = false;
        kData.hasShadowDash = false;
        kData.hasWalljump = hData.hasWalljump;
        kData.hasDoubleJump = hData.hasDoubleJump;
        kData.hasSuperDash = hData.hasBrolly;

        // spells
        if (hData.hasSilkCharge)
            kData.fireballLevel = 2;
        else if (hData.hasNeedleThrow)
            kData.fireballLevel = 1;
        else
            kData.fireballLevel = 0;

        if (hData.hasSilkBossNeedle)
            kData.quakeLevel = 2;
        else if (hData.hasParry)
            kData.quakeLevel = 1;
        else
            kData.quakeLevel = 0;

        if (hData.hasSilkBomb)
            kData.screamLevel = 2;
        else if (hData.hasThreadSphere)
            kData.screamLevel = 1;
        else
            kData.screamLevel = 0;


        // upgrades
        kData.nailDamage = hData.nailDamage;
        kData.maxHealth = hData.maxHealth;
        kData.MPReserve = hData.silkSpoolParts / 2;

        // misc
        kData.hasDreamNail = hData.hasNeedolin;
        kData.hasDreamGate = hData.UnlockedFastTravelTeleport;
        kData.permadeathMode = (int)hData.permadeathMode;
        kData.bossRushMode = hData.bossRushMode;
        kData.salubraBlessing = hData.HasBoundCrestUpgrader;

        kData.hasCyclone = hData.hasChargeSlash;
        kData.hasDashSlash = hData.hasChargeSlash;
        kData.hasUpwardSlash = hData.hasChargeSlash;
        kData.hasNailArt = hData.hasChargeSlash;*/

        // interesting code for syncing Hornet's and the Knight's health/soul

        // if (kData.health != oldKnightHealth)
        // {
        //     hData.health = kData.health;
        // }

        // if (kData.MPCharge != oldKnightMPCharge)
        // {
        //     hData.silk = kData.MPCharge / 11;
        //     knightSoulRemainder = kData.MPCharge % 11;
        // }

        // kData.health = hData.health;
        // kData.MPCharge = hData.silk * 11 + knightSoulRemainder;

        // oldKnightHealth = kData.health;
        // oldKnightMPCharge = kData.MPCharge;


        // fixes
        if (SceneManager.GetActiveScene().name.ToLower() == "song_tower_01")
        {
            if (!managedFsmChange)
                managedFsmChange = bypassSilkHeartLace();
        }
        else if (SceneManager.GetActiveScene().name.ToLower() == "cradle_03")
        {
            if (!managedFsmChange)
                managedFsmChange = fixEndSequence();
        }
        else
            managedFsmChange = false;
    }

    private static bool bypassSilkHeartLace()
    {
        GameObject ob = GameObject.Find("Silk Heart");

        if (ob == null)
            return false;

        // only gets here once the silk heart is active
        GameObject gate = GameObject.Find("song_tower_right_gate");

        gate.GetComponent<Gate>().Open();


        return true;
    }

    private static bool fixEndSequence()
    {
        GameObject ob = GameObject.Find("Death Sequence");

        if (ob == null)
            return false;

        PlayMakerFSM fsm = ob.GetFsm("Control");

        if (fsm.ActiveStateName == "Final Bind")
        {
            // back to hornet
            if (KnightInSilksong.IsKnight)
                KnightInSilksong.shouldToggleKnight = true;

            return true;
        }

        return false;
    }

    private static void patchIntroCutscenes()
    {
        if (PlayerData.instance.bindCutscenePlayed == true)
        {
            return;
        }
        PlayerData.instance.bindCutscenePlayed = true;

        // put knight high up
        HeroController.instance.transform.position = new Vector2(50f, 30f);


        KnightInSilksong.shouldToggleKnight = true;
    }

    private static void disableWeaknessCutscene()
    {
        GameObject weaknessCutscene = GameObject.Find("Weakness Scene");
        weaknessCutscene.active = false;
    }

    private static void movePsalmCylinderDown()
    {
        GameObject pickup = GameObject.Find("Collectable Item Pickup - Melody");
        if (pickup == null)
            return;

        pickup.transform.localPosition = new Vector2(pickup.transform.position.x, 3.6264f);
    }

    private static void moveHang01RingDown()
    {
        GameObject ring = GameObject.Find("Harpoon Ring Citadel (1)");
        if (ring == null)
            return;

        ring.transform.localPosition = new Vector2(ring.transform.position.x, 18.3f);
    }
}
