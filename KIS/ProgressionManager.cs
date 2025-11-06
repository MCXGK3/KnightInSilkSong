using KIS;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProgressionManager
{
    private static GameObject smallPlatform;

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
            placePlatform(54f, 63f);
        }
        if (scene == "mosstown_01")
            placePlatform(30.5f, 16f);
    }

    public static void setProgression()
    {
        // movement
        Knight.PlayerData.instance.hasDash = PlayerData.instance.hasDash;
        Knight.PlayerData.instance.canDash = PlayerData.instance.hasDash;
        Knight.PlayerData.instance.hasWalljump = PlayerData.instance.hasWalljump;
        Knight.PlayerData.instance.hasDoubleJump = PlayerData.instance.hasDoubleJump;
        Knight.PlayerData.instance.hasSuperDash = PlayerData.instance.hasHarpoonDash;

        // spells
        if (PlayerData.instance.hasNeedleThrow)
        {
            Knight.PlayerData.instance.fireballLevel = 1;
        }
        else
        {
            Knight.PlayerData.instance.fireballLevel = 0;
        }

        // upgrades
        Knight.PlayerData.instance.nailDamage = PlayerData.instance.nailDamage;
        Knight.PlayerData.instance.maxHealth = PlayerData.instance.maxHealth;
        Knight.PlayerData.instance.MPReserve = PlayerData.instance.silkSpoolParts / 2;

        // misc
        Knight.PlayerData.instance.hasDreamNail = PlayerData.instance.hasNeedolin;
        Knight.PlayerData.instance.permadeathMode = (int)PlayerData.instance.permadeathMode;
        Knight.PlayerData.instance.bossRushMode = PlayerData.instance.bossRushMode;

    }
}
