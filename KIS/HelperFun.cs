using System.IO;
using BepInEx;
using GlobalEnums;
using HutongGames.PlayMaker;
using KIS;
public enum Tool
{
    Silk_Spear,
    Thread_Sphere,
    Parry,
    Silk_Charge,
    Silk_Bomb,
    Silk_Boss_Needle,
    Straight_Pin,
    Tri_Pin,
    Sting_Shard,
    Tack,
    Harpoon,
    Curve_Claws,
    Curve_Claws_Upgraded,
    Shakra_Ring,
    Pimpilo,
    Conch_Drill,
    WebShot_Forge,
    WebShot_Architect,
    WebShot_Weaver,
    Screw_Attack,
    Cogwork_Saw,
    Cogwork_Flier,
    Rosary_Cannon,
    Lightning_Rod,
    Flintstone,
    Silk_Snare,
    Flea_Brew,
    Lifeblood_Syringe,
    Extractor,
    Mosscreep_Tool_1,
    Mosscreep_Tool_2,
    Lava_Charm,
    Bell_Bind,
    Poison_Pouch,
    Fractured_Mask,
    Multibind,
    White_Ring,
    Brolly_Spike,
    Quickbind,
    Spool_Extender,
    Reserve_Bind,
    Dazzle_Bind,
    Dazzle_Bind_Upgraded,
    Revenge_Crystal,
    Thief_Claw,
    Zap_Imbuement,
    Quick_Sling,
    Maggot_Charm,
    Longneedle,
    Wisp_Lantern,
    Flea_Charm,
    Pinstress_Tool,
    Compass,
    Bone_Necklace,
    Rosary_Magnet,
    Weighted_Anklet,
    Barbed_Wire,
    Dead_Mans_Purse,
    Shell_Satchel,
    Magnetite_Dice,
    Scuttlebrace,
    Wallcling,
    Musician_Charm,
    Sprintmaster,
    Thief_Charm,

}
public enum Charm
{
    WaywardCompass = 2,
    GatheringSwarm = 1,
    StalwartShell = 4,
    SoulCatcher = 20,
    ShamanStone = 19,
    SoulEater = 21,
    Dashmaster = 31,
    Sprintmaster = 37,
    Grubsong = 3,
    GrubberflysElegy = 35,

    UnbreakableHeart = 23,
    UnbreakableGreed = 24,
    UnbreakableStrength = 25,
    SpellTwister = 33,
    SteadyBody = 14,
    HeavyBlow = 15,
    QuickSlash = 32,
    Longnail = 18,
    MarkOfPride = 13,
    FuryOfTheFallen = 6,

    ThornsOfAgony = 12,
    BaldurShell = 5,
    Flukenest = 11,
    DefendersCrest = 10,
    GlowingWomb = 22,
    QuickFocus = 7,
    DeepFocus = 34,
    LifebloodHeart = 8,
    LifebloodCore = 9,
    JonisBlessing = 27,

    Hiveblood = 29,
    SporeShroom = 17,
    SharpShadow = 16,
    ShapeOfUnn = 28,
    NailmastersGlory = 26,
    Weaversong = 39,
    DreamWielder = 30,
    Dreamshield = 38,
    Grimmchild = 40,
    VoidHeart = 36
}
public static class KISHelper
{
    //useful actions
    public static Action OnReturnToMenu = null;
    public static Action OnQuitApp = null;
    //useful Enums
    internal const HeroDeathCocoonTypes knight_death_cocoon = (HeroDeathCocoonTypes)(1 << 30);
    public static string GetSaveDataDirectory(int slot)
    {
        return Path.Combine(Paths.ConfigPath, "shownyoung-KIS", "Slot" + slot);
    }
    public static Texture2D LoadTexture(Stream stream)
    {
        byte[] bytes = new byte[stream.Length];
        stream.Read(bytes, 0, bytes.Length);
        stream.Close();

        // 创建Texture2D并加载图片数据
        Texture2D texture = new Texture2D(2, 2);
        if (texture.LoadImage(bytes))
        {
            return texture;
        }
        return null;
    }
    public static Texture2D LoadTexture(string path)
    {
        // 创建文件流
        FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
        byte[] bytes = new byte[fileStream.Length];
        fileStream.Read(bytes, 0, bytes.Length);
        fileStream.Close();

        // 创建Texture2D并加载图片数据
        Texture2D texture = new Texture2D(2, 2);
        if (texture.LoadImage(bytes))
        {
            return texture;
        }
        return null;
    }
    public static GameObject GetCurrentHero()
    {
        return FsmVariables.GlobalVariables.FindFsmGameObject("Hero").Value;
    }
    public static Component GetComponent(GameObject gameObject, string type)
    {
        return gameObject.GetComponent(type);
    }
    public static Component GetAnyComponent<T1, T2>(this GameObject gameObject) where T1 : Component where T2 : Component
    {
        var c1 = gameObject.GetComponent<T1>();
        if (c1 != null) return c1;
        var c2 = gameObject.GetComponent<T2>();
        if (c2 != null) return c2;
        return null;
    }
    public static Component GetAnotherComponent(this Component monoBehaviour)
    {
        bool is_knight = false;
        var fullName = monoBehaviour.GetType().FullName;
        if (fullName.StartsWith("Knight"))
        {
            is_knight = true;
        }
        var name = fullName.Split(".").Last();
        if (!is_knight) return Knight.HeroController.instance.GetComponent(name);
        else return HeroController.instance.GetComponent(name);
    }
    public static void LogInfo(this object msg)
    {
        if (KnightInSilksong.allowLog.Value)
            KnightInSilksong.logger.LogInfo(msg);
    }
    public static void LogWarning(this object msg)
    {
        if (KnightInSilksong.allowLog.Value)
            KnightInSilksong.logger.LogWarning(msg);
    }
    public static void LogError(this object msg)
    {
        if (KnightInSilksong.allowLog.Value)
            KnightInSilksong.logger.LogError(msg);
    }
    public static void LogDebug(this object msg)
    {
        if (KnightInSilksong.allowLog.Value)
            KnightInSilksong.logger.LogDebug(msg);
    }
    public static void LogFatal(this object msg)
    {
        if (KnightInSilksong.allowLog.Value)
            KnightInSilksong.logger.LogFatal(msg);
    }

    public static string GetToolName(this Tool tool)
    {
        return Enum.GetName(typeof(Tool), tool).Replace("_", " ");
    }
    public static string GetCharmName(this Charm charm)
    {
        return "gotCharm_" + (int)charm;
    }

    public static void CheckForDamageHero(this Knight.HeroBox heroBox, GameObject gameObject)
    {
        DamageHero component = gameObject.GetComponent<DamageHero>();
        if (component != null && !heroBox.heroCtrl.cState.shadowDashing)
        {
            heroBox.damageDealt = component.damageDealt;
            heroBox.hazardType = (int)component.hazardType;
            heroBox.damagingObject = gameObject;
            if (component.OverrideCollisionSide)
            {
                heroBox.collisionSide = component.CollisionSide;
            }
            else
            {
                float num2 = gameObject.transform.position.x;
                float num3 = heroBox.transform.position.x;
                if (component.InvertCollisionSide)
                {
                    float num4 = num3;
                    float num5 = num2;
                    num2 = num4;
                    num3 = num5;
                }

                heroBox.collisionSide = ((!(num2 > num3)) ? CollisionSide.left : CollisionSide.right);
            }
            if (!Knight.HeroBox.IsHitTypeBuffered(heroBox.hazardType))
            {
                heroBox.ApplyBufferedHit();
            }
            else
            {
                heroBox.isHitBuffered = true;
            }
        }
    }
}

