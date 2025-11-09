using KIS;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProgressionManager
{
    private static GameObject smallPlatform;

    private static int oldKnightHealth = 0;
    private static int oldKnightMPCharge = 0;
    private static int knightSoulRemainder = 0;

    public static void setup()
    {
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

    private static void onActiveSceneChanged(Scene from, Scene to)
    {
        String scene = to.name.ToLower();
        if (scene == "tut_02")
            placePlatform(83.5f, 15f);
        if (scene == "tut_03")
            placePlatform(103f, 7f);
        if (scene == "bone_01")
        {
            placePlatform(32f, 12f);
            placePlatform(72.5f, 47.5f);
            placePlatform(62.5f, 57f);
            placePlatform(54f, 65f);
            placePlatform(103f, 72f);
            placePlatform(103f, 82f);
        }
        if (scene == "bone_04")
            placePlatform(75f, 10f);
        if (scene == "mosstown_01")
            placePlatform(30.5f, 16f);
    }

    public static void setProgression()
    {
        PlayerData hData = PlayerData.instance;
        Knight.PlayerData kData = Knight.PlayerData.instance;
        // movement
        kData.hasDash = hData.hasDash;
        kData.canDash = hData.hasDash;
        kData.hasWalljump = hData.hasWalljump;
        kData.hasDoubleJump = hData.hasDoubleJump;
        kData.hasSuperDash = hData.hasHarpoonDash;

        // spells
        if (hData.hasNeedleThrow)
        {
            kData.fireballLevel = 1;
        }
        else
        {
            kData.fireballLevel = 0;
        }

        // upgrades
        kData.nailDamage = hData.nailDamage;
        kData.maxHealth = hData.maxHealth;
        kData.MPReserve = hData.silkSpoolParts / 2;

        // misc
        kData.hasDreamNail = hData.hasNeedolin;
        kData.permadeathMode = (int)hData.permadeathMode;
        kData.bossRushMode = hData.bossRushMode;

        // interesting code for syncing Hornet's and the Knight's health/soul
        if (kData.health != oldKnightHealth)
        {
            Console.WriteLine("CHANGED H HEALTH");
            hData.health = kData.health;
        }

        if (kData.MPCharge != oldKnightMPCharge)
        {
            hData.silk = kData.MPCharge / 11;
            knightSoulRemainder = kData.MPCharge % 11;
        }

        kData.health = hData.health;
        kData.MPCharge = hData.silk * 11 + knightSoulRemainder;

        oldKnightHealth = kData.health;
        oldKnightMPCharge = kData.MPCharge;
    }
}
