using System.IO;
using BepInEx;
using GlobalEnums;
using HutongGames.PlayMaker;
using KIS;
using TeamCherry.Localization;
public enum Tool
{
    /// <summary>
    /// 丝之矛
    /// </summary>
    Silk_Spear,
    /// <summary>
    /// 灵丝风暴
    /// </summary>
    Thread_Sphere,
    /// <summary>
    /// 十字绣
    /// </summary>
    Parry,
    /// <summary>
    /// 丝刃冲刺
    /// </summary>
    Silk_Charge,
    /// <summary>
    /// 符文之怒
    /// </summary>
    Silk_Bomb,
    /// <summary>
    /// 苍白之爪
    /// </summary>
    Silk_Boss_Needle,
    /// <summary>
    /// 直针
    /// </summary>
    Straight_Pin,
    /// <summary>
    /// 三重镖
    /// </summary>
    Tri_Pin,
    /// <summary>
    /// 蜇刺碎片
    /// </summary>
    Sting_Shard,
    /// <summary>
    /// 钉刺
    /// </summary>
    Tack,
    /// <summary>
    /// 长针
    /// </summary>
    Harpoon,
    /// <summary>
    /// 弧爪
    /// </summary>
    Curve_Claws,
    /// <summary>
    /// 曲镰
    /// </summary>
    Curve_Claws_Upgraded,
    /// <summary>
    /// 投掷环
    /// </summary>
    Shakra_Ring,
    /// <summary>
    /// 爆燃囊
    /// </summary>
    Pimpilo,
    /// <summary>
    /// 螺切刃
    /// </summary>
    Conch_Drill,
    /// <summary>
    /// 丝弹-铁匠
    /// </summary>
    WebShot_Forge,
    /// <summary>
    /// 丝弹-建筑师
    /// </summary>
    WebShot_Architect,
    /// <summary>
    /// 丝弹-编织者
    /// </summary>
    WebShot_Weaver,
    /// <summary>
    /// 掘洞钻
    /// </summary>
    Screw_Attack,
    /// <summary>
    /// 机轮刃
    /// </summary>
    Cogwork_Saw,
    /// <summary>
    /// 齿轮蜂
    /// </summary>
    Cogwork_Flier,
    /// <summary>
    /// 念珠炮
    /// </summary>
    Rosary_Cannon,
    /// <summary>
    /// 电枢球
    /// </summary>
    Lightning_Rod,
    /// <summary>
    /// 燧石板
    /// </summary>
    Flintstone,
    /// <summary>
    /// 陷阱设置器
    /// </summary>
    Silk_Snare,
    /// <summary>
    /// 跳蚤秘酿
    /// </summary>
    Flea_Brew,
    /// <summary>
    /// 生质液瓶
    /// </summary>
    Lifeblood_Syringe,
    /// <summary>
    /// 储液针管
    /// </summary>
    Extractor,
    /// <summary>
    /// 德鲁伊之眼
    /// </summary>
    Mosscreep_Tool_1,
    /// <summary>
    /// 德鲁伊双瞳
    /// </summary>
    Mosscreep_Tool_2,
    /// <summary>
    /// 熔岩钟
    /// </summary>
    Lava_Charm,
    /// <summary>
    /// 护佑钟
    /// </summary>
    Bell_Bind,
    /// <summary>
    /// 花芯囊
    /// </summary>
    Poison_Pouch,
    /// <summary>
    /// 裂痕面具
    /// </summary>
    Fractured_Mask,
    /// <summary>
    /// 多重缚丝器
    /// </summary>
    Multibind,
    /// <summary>
    /// 织光仪
    /// </summary>
    White_Ring,
    /// <summary>
    /// 锯齿环
    /// </summary>
    Brolly_Spike,
    /// <summary>
    /// 注丝套针
    /// </summary>
    Quickbind,
    /// <summary>
    /// 储丝延展器
    /// </summary>
    Spool_Extender,
    /// <summary>
    /// 储备缚丝
    /// </summary>
    Reserve_Bind,
    /// <summary>
    /// 爪镜
    /// </summary>
    Dazzle_Bind,
    /// <summary>
    /// 双生爪镜
    /// </summary>
    Dazzle_Bind_Upgraded,
    /// <summary>
    /// 记忆晶石
    /// </summary>
    Revenge_Crystal,
    /// <summary>
    /// 撬赃钩
    /// </summary>
    Thief_Claw,
    /// <summary>
    /// 伏特丝
    /// </summary>
    Zap_Imbuement,
    /// <summary>
    /// 速射索
    /// </summary>
    Quick_Sling,
    /// <summary>
    /// 净界花环
    /// </summary>
    Maggot_Charm,
    /// <summary>
    /// 长爪
    /// </summary>
    Longneedle,
    /// <summary>
    /// 灵火提灯
    /// </summary>
    Wisp_Lantern,
    /// <summary>
    /// 蚤母卵
    /// </summary>
    Flea_Charm,
    /// <summary>
    /// 针徽
    /// </summary>
    Pinstress_Tool,
    /// <summary>
    /// 罗盘
    /// </summary>
    Compass,
    /// <summary>
    /// 碎壳坠
    /// </summary>
    Bone_Necklace,
    /// <summary>
    /// 磁石胸针
    /// </summary>
    Rosary_Magnet,
    /// <summary>
    /// 负重环带
    /// </summary>
    Weighted_Anklet,
    /// <summary>
    /// 棘刺手环
    /// </summary>
    Barbed_Wire,
    /// <summary>
    /// 亡虫囊
    /// </summary>
    Dead_Mans_Purse,
    /// <summary>
    /// 壳囊
    /// </summary>
    Shell_Satchel,
    /// <summary>
    /// 磁石骰
    /// </summary>
    Magnetite_Dice,
    /// <summary>
    /// 迅捷脊锁
    /// </summary>
    Scuttlebrace,
    /// <summary>
    /// 登极握爪
    /// </summary>
    Wallcling,
    /// <summary>
    /// 蛛丝弦
    /// </summary>
    Musician_Charm,
    /// <summary>
    /// 丝速脚环
    /// </summary>
    Sprintmaster,
    /// <summary>
    /// 窃者印记
    /// </summary>
    Thief_Charm,

}
public enum Charm
{
    /// <summary>
    /// 任性的指南针
    /// </summary>
    WaywardCompass = 2,
    /// <summary>
    /// 蜂群集结
    /// </summary>
    GatheringSwarm = 1,
    /// <summary>
    /// 坚硬外壳
    /// </summary>
    StalwartShell = 4,
    /// <summary>
    /// 灵魂捕手
    /// </summary>
    SoulCatcher = 20,
    /// <summary>
    /// 萨满之石
    /// </summary>
    ShamanStone = 19,
    /// <summary>
    /// 噬魂者
    /// </summary>
    SoulEater = 21,
    /// <summary>
    /// 冲刺大师
    /// </summary>
    Dashmaster = 31,
    /// <summary>
    /// 飞毛腿
    /// </summary>
    Sprintmaster = 37,
    /// <summary>
    /// 幼虫之歌
    /// </summary>
    Grubsong = 3,
    /// <summary>
    /// 蜕变挽歌
    /// </summary>
    GrubberflysElegy = 35,
    /// <summary>
    ///  坚固心脏
    /// </summary>
    UnbreakableHeart = 23,
    /// <summary>
    ///  坚固贪婪
    /// </summary>
    UnbreakableGreed = 24,
    /// <summary>
    ///  坚固力量
    /// </summary>
    UnbreakableStrength = 25,
    /// <summary>
    /// 法术扭曲者
    /// </summary>
    SpellTwister = 33,
    /// <summary>
    /// 稳定之体
    /// </summary>
    SteadyBody = 14,
    /// <summary>
    /// 沉重之击
    /// </summary>
    HeavyBlow = 15,
    /// <summary>
    /// 快速劈砍
    /// </summary>
    QuickSlash = 32,
    /// <summary>
    /// 修长之钉
    /// </summary>
    Longnail = 18,
    /// <summary>
    /// 骄傲印记
    /// </summary>
    MarkOfPride = 13,
    /// <summary>
    /// 亡者之怒
    /// </summary>
    FuryOfTheFallen = 6,
    /// <summary>
    /// 苦痛荆棘
    /// </summary>
    ThornsOfAgony = 12,
    /// <summary>
    /// 巴德尔之壳
    /// </summary>
    BaldurShell = 5,
    /// <summary>
    /// 吸虫之巢
    /// </summary>
    Flukenest = 11,
    /// <summary>
    /// 防御者纹章
    /// </summary>
    DefendersCrest = 10,
    /// <summary>
    /// 发光子宫
    /// </summary>
    GlowingWomb = 22,
    /// <summary>
    /// 快速凝聚
    /// </summary>
    QuickFocus = 7,
    /// <summary>
    /// 深度凝聚
    /// </summary>
    DeepFocus = 34,
    /// <summary>
    /// 生命血之心
    /// </summary>
    LifebloodHeart = 8,
    /// <summary>
    /// 生命血核心
    /// </summary>
    LifebloodCore = 9,
    /// <summary>
    /// 乔尼的祝福
    /// </summary>
    JonisBlessing = 27,
    /// <summary>
    /// 蜂巢之血
    /// </summary>
    Hiveblood = 29,
    /// <summary>
    /// 蘑菇孢子
    /// </summary>
    SporeShroom = 17,
    /// <summary>
    /// 锋利之影
    /// </summary>
    SharpShadow = 16,
    /// <summary>
    /// 乌恩之形
    /// </summary>
    ShapeOfUnn = 28,
    /// <summary>
    /// 骨钉大师的荣耀
    /// </summary>
    NailmastersGlory = 26,
    /// <summary>
    /// 编织者之歌
    /// </summary>
    Weaversong = 39,
    /// <summary>
    /// 舞梦者
    /// </summary>
    DreamWielder = 30,
    /// <summary>
    /// 梦之盾
    /// </summary>
    Dreamshield = 38,
    /// <summary>
    /// 格林之子
    /// </summary>
    Grimmchild = 40,
    /// <summary>
    /// 虚空之心
    /// </summary>
    VoidHeart = 36
}
public enum Spell
{
    Scream,
    Fireball,
    Quake
}
public enum NailArt
{
    GREAT_SLASH,
    DASH_SLASH,
    CYCLONE
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
    public static Type[] GetTypesSafely(this Assembly asm)
    {
        try
        {
            return asm.GetTypes();
        }
        catch (ReflectionTypeLoadException ex)
        {
            return ex.Types.Where(x => x is not null).ToArray();
        }
    }
    public static bool IsAssignableFromSafely(this Type self, Type other)
    {
        try
        {
            return self.IsAssignableFrom(other);
        }
        catch
        {
            return false;
        }
    }
    public static LocalisedString Localize(this LangKey key)
    {
        return new($"Mods.{KnightInSilksong.Id}", MoreLanguge.GetInGameKey(key));
    }
}

