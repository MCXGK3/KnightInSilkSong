using KIS;

public class ProgressionManager
{
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
