using System.IO;
using Newtonsoft.Json;
using KIS;
using UnityEngine.SceneManagement;
using HutongGames.PlayMaker;
using KIS.Utils;
using TMProOld;
using UnityEngine.Audio;
using GlobalEnums;
using GlobalSettings;
using PDA = PrepatcherPlugin.PlayerDataAccess;

internal class PreProcess : IModule
{
    public PreProcess()
    {
        Instance = this;
    }
    public static PreProcess Instance;
    internal bool shader_initialized = false;
    private Dictionary<string, string> material_shader_map;
    private Dictionary<string, Shader> shaders = new();
    public Sprite charm_icon;
    public HashSet<AudioMixer> share_mixer_group = new();




    public override void Init()
    {
        using (StreamReader reader = new(Assembly.GetExecutingAssembly().GetManifestResourceStream("KIS.Resources.MaterialShaderMap.json")))
        {
            string json = reader.ReadToEnd();
            material_shader_map = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        }
        charm_icon = KnightInSilksong.Instance.hk.LoadAsset<Sprite>("GG_bound_charm_prompt");
        UnityEngine.SceneManagement.SceneManager.activeSceneChanged += CheckShaders;
        SSizeKnight();
        SSizeHud();
        SSizeCharm();
        SSizetk2dCollectionData();
        SetAuidoVolume();


    }

    private void SSizetk2dCollectionData()
    {
        foreach (var go_pair in KnightInSilksong.loaded_gos)
        {
            var go = go_pair.Value;
            foreach (var collection in go.GetComponentsInChildren<tk2dSpriteCollectionData>(true))
            {
                collection.spriteCollectionName = collection.spriteCollectionName + "(Hollownest)";
                collection.name = collection.spriteCollectionName;
            }
        }
    }

    private void SSizeCharm()
    {
        if (KnightInSilksong.Instance.charm == null) return;
        var charm = KnightInSilksong.Instance.charm;
        foreach (var render in charm.GetComponentsInChildren<Renderer>(true))
        {
            render.sortingGroupID = 1048575;
            render.sortingLayerID = 59515797;
        }
        var build = charm.GetComponentInChildren<BuildEquippedCharms>(true);
        if (build != null)
        {
            foreach (var charm_prefab in build.gameObjectList)
            {
                foreach (var render in charm_prefab.GetComponentsInChildren<Renderer>(true))
                {
                    render.sortingGroupID = 1048575;
                    render.sortingLayerID = 59515797;
                }
            }
        }
        var equip_msg = KnightInSilksong.loaded_gos["Charm Equip Msg"];
        foreach (var render in equip_msg.GetComponentsInChildren<Renderer>(true))
        {
            render.sortingGroupID = 1048575;
            render.sortingLayerID = 59515797;
        }
    }

    private void SSizeHud()
    {
        if (KnightInSilksong.Instance.hud_canvas == null) return;
        var hud = KnightInSilksong.Instance.hud_canvas;
        hud.LocateMyFSM("Globalise").enabled = true;
        hud.FindGameObjectInChildren("Geo Counter").SetActive(false);


    }

    private void SSizeKnight()
    {

        if (KnightInSilksong.Instance.knight == null) return;
        var knight = KnightInSilksong.Instance.knight;
        SetErrorCollider2D();
        SetErrorFsm();
        knight.GetComponent<tk2dSpriteAnimator>().Library.GetClipByName("Prostrate Rise").frames[1].triggerEvent = true;
        knight.GetComponent<tk2dSpriteAnimator>().Library.GetClipByName("Prostrate Rise").frames[18].triggerEvent = true;
        knight.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        knight.AddComponent<KeepHornet>();
        knight.AddComponent<DreamHelper>();


        KnightInSilksong.loaded_gos["Grimmchild"].tag = "Grimmchild";
        KnightInSilksong.loaded_gos["Weaverling"].tag = "Weaverling";
        KnightInSilksong.loaded_gos["Knight Hatchling"].tag = "Knight Hatchling";
        KnightInSilksong.loaded_gos["Orbit Shield"].tag = "Orbit Shield";









    }

    private void CheckShaders(Scene arg0, Scene arg1)
    {
        SetShaders();
        if (shaders.ContainsKey("Sprites/Default-ColorFlash") && !shader_initialized)
        {
            shader_initialized = true;
            KnightInSilksong.logger.LogInfo("Shader Find OK");
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged -= CheckShaders;
            SetErrorShaders();

        }
    }
    private void SetShaders()
    {
        foreach (var shader in Resources.FindObjectsOfTypeAll<Shader>())
        {
            if (shader != null && shader.isSupported)
            {
                if (!shaders.ContainsKey(shader.name))
                {
                    KnightInSilksong.logger.LogInfo(shader.name + " " + shader.isSupported);
                    shaders.Add(shader.name, shader);
                }
            }
        }
    }

    public Shader GetShader(string name)
    {
        return shaders.ContainsKey(name) ? shaders[name] : null;
    }
    void SetErrorShaders()
    {
        var hk = KnightInSilksong.Instance.hk;
        HashSet<Material> materials = new();


        foreach (var go_pair in KnightInSilksong.loaded_gos)
        {
            var go = go_pair.Value;
            foreach (var renderer in go.GetComponentsInChildren<SpriteRenderer>(true))
            {
                materials.Add(renderer.sharedMaterial);
            }
            foreach (var renderer in go.GetComponentsInChildren<ParticleSystemRenderer>(true))
            {
                materials.Add(renderer.sharedMaterial);
            }
            foreach (var data in go.GetComponentsInChildren<tk2dSpriteCollectionData>(true))
            {
                foreach (var material in data.materials)
                {
                    materials.Add(material);
                }
            }
            foreach (var changefont in go.GetComponentsInChildren<ChangeFontByLanguage>(true))
            {
                if (changefont.fontJA != null)
                    materials.Add(changefont.fontJA.material);
                if (changefont.fontKO != null)
                    materials.Add(changefont.fontKO.material);
                if (changefont.fontRU != null)
                    materials.Add(changefont.fontRU.material);
                if (changefont.fontZH != null)
                    materials.Add(changefont.fontZH.material);
                if (changefont.defaultFont != null)
                    materials.Add(changefont.defaultFont.material);
            }
            foreach (var text in go.GetComponentsInChildren<TextMeshPro>(true))
            {
                foreach (var material in text.fontSharedMaterials)
                {
                    materials.Add(material);
                }
            }
        }
        foreach (var material in materials)
        {
            if (material == null)
            {
                "Null Material".LogInfo();
                continue;
            }
            if (!material_shader_map.ContainsKey(material.name))
            {
                KnightInSilksong.logger.LogError("Cant Find The Material " + material.name + " In Map");
                continue;
            }
            var shader = GetShader(material_shader_map[material.name]);
            if (shader == null)
            {
                switch (material_shader_map[material.name])
                {
                    case "tk2d/BlendVertexColor":
                        shader = GetShader("tk2d/BlendVertexColor (addressable)");
                        break;
                    case "UI/BlendModes/Lighten":
                        shader = GetShader("UI/BlendModes/Screen");
                        break;
                    case "UI/BlendModes/Multiply":
                        shader = GetShader("UI/BlendModes/Screen");
                        break;
                    case "UI/BlendModes/VividLight":
                        shader = GetShader("UI/BlendModes/Screen");
                        break;
                    default:
                        break;
                }
                if (shader == null)
                {
                    KnightInSilksong.logger.LogError("Cant Find The Shader " + material_shader_map[material.name] + " For " + material.name);
                }
            }
            material.shader = shader;
            // if (shader.name == "Alpha Masked/Sprites Alpha Masked - World Coords")
            // {
            //     material.SetTexture("_AlphaTex", orb_full);
            // }
        }

    }
    void SetErrorCollider2D()
    {
        foreach (var go_pair in KnightInSilksong.loaded_gos)
        {
            var go = go_pair.Value;
            foreach (var collider in go.GetComponentsInChildren<Collider2D>(true))
            {
                collider.callbackLayers = -1;
                collider.contactCaptureLayers = -1;
                collider.forceReceiveLayers = -1;
                collider.forceSendLayers = -1;
            }
        }
    }
    void SetAuidoVolume()
    {
        foreach (var go_pair in KnightInSilksong.loaded_gos)
        {
            var go = go_pair.Value;
            foreach (var audioSource in go.GetComponentsInChildren<AudioSource>(true))
            {
                if (audioSource != null && audioSource.outputAudioMixerGroup != null && audioSource.outputAudioMixerGroup.audioMixer != null)
                    share_mixer_group.Add(audioSource.outputAudioMixerGroup.audioMixer);
            }
        }
    }
    void SetErrorFsm()
    {
        foreach (var go_pair in KnightInSilksong.loaded_gos)
        {
            var go = go_pair.Value;
            foreach (var fsm in go.GetComponentsInChildren<PlayMakerFSM>(true))
            {
                fsm.eventHandlerComponentsAdded = false;
                if (fsm.UsesTemplate)
                {
                    fsm.fsmTemplate.fsm.GetAddVariable<FsmBool>("FromKnight", true);
                    fsm.fsmTemplate.fsm.preprocessed = false;
                }
                else
                {
                    fsm.GetAddVariable<FsmBool>("FromKnight", true);
                    fsm.fsm.preprocessed = false;
                }

            }
        }

    }
    public static void SetDeathInfo()
    {
        HeroCorpseMarkerProxy instance = HeroCorpseMarkerProxy.Instance;
        HeroController hc = HeroController.instance;
        if (instance)
        {
            PDA.HeroCorpseScene = instance.TargetSceneName;
            PDA.HeroCorpseMarkerGuid = instance.TargetGuid;
            PDA.HeroDeathScenePos = instance.TargetScenePos;
        }
        else
        {
            Vector3 position = hc.transform.position;
            PDA.HeroCorpseScene = hc.gm.GetSceneNameString();
            HeroCorpseMarker closest = HeroCorpseMarker.GetClosest(position);
            if (closest)
            {
                PDA.HeroCorpseMarkerGuid = closest.Guid.ToByteArray();
                PDA.HeroDeathScenePos = closest.Position;
            }
            else
            {
                PDA.HeroCorpseMarkerGuid = null;
                PDA.HeroDeathScenePos = position;
            }
        }
        tk2dTileMap tilemap = hc.gm.tilemap;
        PDA.HeroDeathSceneSize = new Vector2((float)tilemap.width, (float)tilemap.height);
        hc.gm.gameMap.PositionCompassAndCorpse();
        PDA.IsSilkSpoolBroken = true;
        PDA.HeroCorpseType = HeroDeathCocoonTypes.Normal;
        int num = Knight.PlayerData.instance.geoPool;//hc.playerData.geo;
        bool isEquipped = Gameplay.DeadPurseTool.IsEquipped;
        if (isEquipped)
        {
            int num2 = Mathf.RoundToInt((float)num * Gameplay.DeadPurseHoldPercent);
            num -= num2;
            PDA.geo = num2;
        }
        else
        {
            PDA.geo = 0;
        }
        PDA.HeroCorpseMoneyPool = Mathf.RoundToInt((float)num);
        if (hc.playerData.IsAnyCursed)
        {
            PDA.HeroCorpseType |= HeroDeathCocoonTypes.Cursed;
        }
        if (isEquipped && PDA.HeroCorpseMoneyPool >= 10)
        {
            PDA.HeroCorpseType |= HeroDeathCocoonTypes.Rosaries;
        }

        PDA.HeroCorpseType |= KISHelper.knight_death_cocoon;
        Knight.PlayerData kd = Knight.PlayerData.instance;
        kd.shadeScene = PDA.HeroCorpseScene;
        kd.shadePositionX = PDA.HeroDeathScenePos.x;
        kd.shadePositionY = PDA.HeroDeathScenePos.y;
        kd.geoPool = PDA.HeroCorpseMoneyPool;
    }
}
