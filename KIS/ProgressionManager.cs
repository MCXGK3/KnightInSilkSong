using KIS;
using KIS.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;

namespace KIS;

public class ProgressionManager
{
    private static GameObject smallPlatform;

    private static int oldKnightHealth = 0;
    private static int oldKnightMPCharge = 0;
    private static int knightSoulRemainder = 0;

    private static bool managedFsmChange = false;

    private static SyncManager syncManager = new();

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
        plat.GetComponent<Transform>().position = pos;
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
        String scene = to.name.ToLower();
        managedFsmChange = false;

        // fixes
        if (scene == "tut_01")
        {
            patchIntroCutscenes();
            disableWeaknessCutscene();
        }
        if (scene == "tut_03")
            disableWeaknessCutscene();
        if (scene == "bonetown")
        {
            PlayerData.instance.churchKeeperIntro = true;
            disableWeaknessCutscene();
        }
        if (scene == "library_10")
            movePsalmCylinderDown();

        // platforms
        if (scene == "tut_02")
            placePlatform(83.5f, 15f);
        if (scene == "tut_03")
            placePlatform(103f, 7f);
        if (scene == "bone_01")
        {
            placePlatform(32f, 12f);
            placePlatform(72.5f, 47.5f);
            placePlatform(62.5f, 57f);
            placePlatform(54f, 64f);
            placePlatform(103f, 71.5f);
            placePlatform(103f, 82f);
        }
        if (scene == "bone_04")
            placePlatform(75f, 10f);
        if (scene == "bone_08")
        {
            placePlatform(19.5f, 25f);
            placePlatform(13.5f, 39f);
            placePlatform(11f, 44f);
            placePlatform(14.7f, 53.5f);
        }
        if (scene == "mosstown_01")
            placePlatform(30.5f, 16f);
        if (scene == "bone_east_01")
            placePlatform(11f, 29f);
        if (scene == "crawl_03")
            placePlatform(171f, 62f);
        if (scene == "crawl_01")
            placePlatform(55f, 51f);
        if (scene == "aspid_01")
            placePlatform(57f, 19f);

        if (scene == "shellwood_03")
            placeBounceBloom(10f, 21.5f, "Shellwood Bounce Bloom");
        if (scene == "shellwood_10")
            placePlatform(65f, 14f);
        if (scene == "shellwood_13")
            placePlatform(77f, 8f);
        if (scene == "under_17")
            placePlatform(7f, 7f);
        if (scene == "hang_01")
            moveHang01RingDown();
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
