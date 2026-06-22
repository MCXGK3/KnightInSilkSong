using System;
using System.Collections;
using DebugMod;
using DebugMod.UI;
using DebugMod.UI.Canvas;
using DebugMod.SaveStates;
using System.IO;

namespace KIS.Compatibility
{
    [RequiresMod(DebugMod.DebugMod.Id)]
    public class WithDebugMod : ICompatibility
    {
        public string ModName => "Debug Mod";

        public string ModId => DebugMod.DebugMod.Id;

        static Knight.PlayerData pd => Knight.PlayerData.instance;
        static Knight.HeroController hc => Knight.HeroController.instance;
        private static PlayMakerFSM _refKnightSlash;
        internal static PlayMakerFSM RefKnightSlash => _refKnightSlash != null ? _refKnightSlash : (_refKnightSlash = hc?.transform.Find("Attacks/Slash").GetComponent<PlayMakerFSM>());
        public static bool infinite_jump = false;


        public void Init()
        {
            DebugMod.DebugMod.Log("Debug Mod detected KIS, initializing compatibility.");

            KnightInSilksong.Instance.self_hormony.PatchAll(typeof(WithDebugMod));

            DebugMod.DebugMod.AddTranslationSheet($"Mods.io.github.shownyoung.knightinsilksong");

            SaveState.OnSave += OnSave;
            SaveState.AfterLoad += AfterLoad;

            DebugMod.DebugMod.AddTextToInfoPanel("Character",
                () => KnightInSilksong.IsKnight ? "Knight" : "Hornet",
                DebugMod.DebugMod.InfoPanelColumn.RIGHT
            );

            RegisterBindableFunctions();

            //TODO: insert tab before keybinds tab so we don't mess up the structure :(
            // we can hack this now with an ILManipulator or just wait for DebugMod to expose nicer ways to create UI
        }

        public void Update()
        {
            if (!KnightInSilksong.IsKnight) return;
            if (DebugMod.DebugMod.noclip)
            {
                if (hc?.transitionState == GlobalEnums.HeroTransitionState.WAITING_TO_TRANSITION && SaveState.loadingSavestate == null)
                {
                    hc?.transform.position = DebugMod.DebugMod.noclipPos;
                    // hc.GetComponent<Rigidbody2D>().constraints |= RigidbodyConstraints2D.FreezePosition;
                }
                else
                {
                    DebugMod.DebugMod.noclipPos = hc.transform.position;
                    // hc.GetComponent<Rigidbody2D>().constraints &= ~RigidbodyConstraints2D.FreezePosition;
                }
            }
            if (DebugMod.DebugMod.infiniteSilk)
            {
                if (pd.MPCharge < pd.maxMP)
                {
                    pd?.MPCharge = pd.maxMP;
                    hc?.SoulGain();
                }
            }
            if (DebugMod.DebugMod.playerInvincible)
            {
                pd?.isInvincible = true;
            }
            Knight.HeroBox.inactive = DebugMod.DebugMod.heroColliderDisabled;
            infinite_jump = PlayerData.instance?.infiniteAirJump ?? false;
        }

        #region SaveState
        private static void OnSave(SaveState state)
        {
            if (!KnightInSilksong.IsKnight) return;
            state.data.customData["KIS.KnightPd"] = JsonUtility.ToJson(pd);
        }

        private static void AfterLoad(SaveState state)
        {
            var stateIsKnight = state.data.customData.TryGetValue("KIS.KnightPd", out var statePdJson);
            if (KnightInSilksong.IsKnight != stateIsKnight) KnightInSilksong.Instance.ToggleKnight();
            if (!stateIsKnight) return;

            JsonUtility.FromJsonOverwrite(JsonUtility.ToJson(statePdJson), pd);

            hc.transform.position = state.data.savePos;

            var hud = KnightInSilksong.Instance.hud_instance.gameObject;
            FSMUtility.SendEventToGameObject(hud, "HERO LEAVE", true);
            FSMUtility.SendEventToGameObject(hud, "HERO DAMAGED", true);
            hud.transform.Find("Soul Orb/Orb Full").gameObject.SetActive(false);
            var soul = hud.transform.Find("Soul Orb").gameObject;
            // soul.SetActive(false);
            // soul.SetActive(true);
            FSMUtility.SendEventToGameObject(soul, "MP RESERVE DOWN", true);
            FSMUtility.SendEventToGameObject(soul,
                pd.GetInt(nameof(pd.MPCharge)) >= pd.GetInt(nameof(pd.maxMP)) ? "MP GAIN" : "MP LOSE", true);

            hc!.CancelDamageRecoil();
            var invPulse = hc!.GetComponent<InvulnerablePulse>();
            invPulse?.StopInvulnerablePulse();
            hc.transitionState = GlobalEnums.HeroTransitionState.WAITING_TO_TRANSITION;
            hc.cState.invulnerable = false;

            // We need to wait a few frames before clearing the HUD,
            // because DebugMod calls HUDFixes after our AfterLoad hooks fire.
            IEnumerator WaitThenClearHud()
            {
                yield return null; yield return null;
                KnightInSilksong.Instance.DisableOriHud();
            }

            hc?.StartCoroutine(WaitThenClearHud());
        }

        /// <summary>
        /// States created with Knight active on versions before 0.6.6 stored playerData separately;
        /// write this into the customData appropriately & delete the old files. 
        /// </summary>
        [HarmonyPatch(typeof(SaveStateManager), nameof(SaveStateManager.LoadFileStates))]
        [HarmonyPostfix]
        private static void UpgradeLegacyStates()
        {
            foreach (var (page, statePage) in SaveStateManager.fileStates)
            {
                for (var i = 0; i < statePage.Length; i++)
                {
                    var state = statePage[i];
                    if (state == null) continue;
                    var filePath = SaveStateManager.GetFilePath(page, i);
                    filePath = Path.Combine(Path.GetDirectoryName(filePath)!, "knightPD_" + Path.GetFileName(filePath));
                    try
                    {
                        if (File.Exists(filePath))
                        {
                            KnightInSilksong.logger.LogInfo($"Found knightPD file for state p{page}[{i}], upgrading...");
                            state.data.customData["KIS.KnightPd"] = File.ReadAllText(filePath);
                            SaveStateManager.SaveToFile(state.data, page, i);
                            File.Delete(filePath);
                        }
                    }
                    catch (Exception ex)
                    {
                        ex.LogDebug();
                    }
                }
            }
        }
        #endregion

        #region Replace Functions
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Knight.PlayerData), nameof(Knight.PlayerData.TakeHealth), MethodType.Normal)]
        public static bool Debug_Knight_PlayerData_TakeHealth_Prefix(Knight.PlayerData __instance, ref int amount)
        {
            if (!KnightInSilksong.IsKnight) return true;
            DebugMod.ModHooks.PlayerData_TakeHealth(ref amount);
            return true;
        }


        [HarmonyPrefix]
        [HarmonyPatch(typeof(BindableFunctions), nameof(BindableFunctions.GiveMask), MethodType.Normal)]
        public static bool BindableFunctions_GiveMask_Prefix()
        {
            if (!KnightInSilksong.IsKnight) return true;
            if (pd?.maxHealthBase < 9)
            {
                pd.MaxHealth();
                pd.AddToMaxHealth(1);
                PlayMakerFSM.BroadcastEvent("MAX HP UP");
            }
            return false;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(BindableFunctions), nameof(BindableFunctions.GiveSpool), MethodType.Normal)]
        public static bool BindableFunctions_GiveSpool_Prefix()
        {
            if (!KnightInSilksong.IsKnight) return true;
            if (pd?.MPReserveMax < 99)
            {
                hc?.AddToMaxMPReserve(33);
                PlayMakerFSM.BroadcastEvent("NEW SOUL ORB");
            }
            return false;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(BindableFunctions), nameof(BindableFunctions.TakeAwayMask), MethodType.Normal)]
        public static bool BindableFunctions_TakeAwayMask_Prefix()
        {
            if (!KnightInSilksong.IsKnight) return true;
            if (pd?.maxHealthBase > 1)
            {
                pd.maxHealth -= 1;
                pd.maxHealthBase -= 1;
                if (!KnightInSilksong.Instance.hud_instance.gameObject.activeInHierarchy)
                    KnightInSilksong.Instance.hud_instance.gameObject.SetActive(true);
                else
                {
                    KnightInSilksong.Instance.hud_instance.gameObject.SetActive(false);
                    KnightInSilksong.Instance.hud_instance.gameObject.SetActive(true);
                }
            }
            return false;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(BindableFunctions), nameof(BindableFunctions.TakeAwaySpool), MethodType.Normal)]
        public static bool BindableFunctions_TakeAwaySpool_Prefix()
        {
            if (!KnightInSilksong.IsKnight) return true;
            if (pd?.MPReserveMax > 0)
            {
                pd?.MPReserveMax -= 33;
                if (!KnightInSilksong.Instance.hud_instance.gameObject.activeInHierarchy)
                    KnightInSilksong.Instance.hud_instance.gameObject.SetActive(true);
                else
                {
                    KnightInSilksong.Instance.hud_instance.gameObject.SetActive(false);
                    KnightInSilksong.Instance.hud_instance.gameObject.SetActive(true);
                }
            }
            return false;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(BindableFunctions), nameof(BindableFunctions.AddHealth), MethodType.Normal)]
        public static bool BindableFunctions_AddHealth_Prefix()
        {
            if (!KnightInSilksong.IsKnight) return true;
            if (pd?.health < pd.maxHealth)
            {
                hc?.AddHealth(1);
            }
            return false;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(BindableFunctions), nameof(BindableFunctions.TakeHealth), MethodType.Normal)]
        public static bool BindableFunctions_TakeHealth_Prefix()
        {
            if (!KnightInSilksong.IsKnight) return true;
            if (pd?.health > 1)
            {
                hc?.TakeHealth(1);
            }
            return false;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(BindableFunctions), nameof(BindableFunctions.AddSilk), MethodType.Normal)]
        public static bool BindableFunctions_AddSilk_Prefix()
        {
            if (!KnightInSilksong.IsKnight) return true;
            hc?.AddMPCharge(33);
            return false;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(BindableFunctions), nameof(BindableFunctions.TakeSilk), MethodType.Normal)]
        public static bool BindableFunctions_TakeSilk_Prefix()
        {
            if (!KnightInSilksong.IsKnight) return true;
            hc?.TakeMP(33);
            return false;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(BindableFunctions), nameof(BindableFunctions.Lifeblood), MethodType.Normal)]
        public static bool BindableFunctions_Lifeblood_Prefix()
        {
            //the logic is same, so do nothing
            return true;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(BindableFunctions), nameof(BindableFunctions.KillSelf), MethodType.Normal)]
        public static bool BindableFunctions_KillSelf_Prefix()
        {
            if (!KnightInSilksong.IsKnight) return false;
            hc?.StartCoroutine(hc.Die());
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(BindableFunctions), nameof(BindableFunctions.GiveAllSkills), MethodType.Normal)]
        public static bool BindableFunctions_GiveAllSkills_Prefix()
        {
            if (!KnightInSilksong.IsKnight) return true;
            GiveAllSkills();
            return false;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(BindableFunctions), nameof(BindableFunctions.IncreaseNeedleDamage), MethodType.Normal)]
        public static bool BindableFunctions_IncreaseNeedleDamage_Prefix()
        {
            if (!KnightInSilksong.IsKnight) return true;
            int num = 4;
            if (pd?.nailDamage == 0)
            {
                num = 5;
            }
            pd?.nailDamage += num;
            PlayMakerFSM.BroadcastEvent("UPDATE NAIL DAMAGE");
            return false;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(BindableFunctions), nameof(BindableFunctions.DecreaseNeedleDamage), MethodType.Normal)]
        public static bool BindableFunctions_DecreaseNeedleDamage_Prefix()
        {
            if (!KnightInSilksong.IsKnight) return true;
            int num = pd.nailDamage - 4;
            pd.nailDamage = Math.Max(0, num);
            PlayMakerFSM.BroadcastEvent("UPDATE NAIL DAMAGE");
            return false;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(BindableFunctions), nameof(BindableFunctions.ToggleHUD), MethodType.Normal)]
        public static bool BindableFunctions_ToggleHUD_Prefix()
        {
            if (!KnightInSilksong.IsKnight) return true;
            if (KnightInSilksong.Instance.hud_instance.gameObject.activeInHierarchy)
            {
                KnightInSilksong.Instance.hud_instance.gameObject.SetActive(false);
            }
            else
            {
                KnightInSilksong.Instance.hud_instance.gameObject.SetActive(true);
            }
            return false;
        }
        #endregion

        #region Adjust InfoPanel
        [HarmonyPostfix]
        [HarmonyPatch(typeof(InfoPanel), nameof(InfoPanel.AppendInfo), [typeof(string), typeof(Func<string>)])]
        public static void InfoPanel_AppendInfo_Postfix(InfoPanel __instance, string label, ref Func<string> info)
        {
            Color silver = new(192f / 255, 192f / 255, 192f / 255);
            int counter = __instance.counter - 1;
            CanvasText labelText = (CanvasText)__instance.byName[$"Label{counter}"];
            CanvasText infoText = (CanvasText)__instance.byName[$"Info{counter}"];
            void KnightLabel(string ori_name, string knight_name)
            {
                labelText.OnUpdate += () =>
                {
                    labelText.Text = KnightInSilksong.IsKnight ? knight_name : ori_name;
                };
            }

            void KnightInfo(Func<string> knight_info)
            {
                labelText.OnUpdate += () =>
                {
                    if (!KnightInSilksong.IsKnight) labelText.Color = Color.white;
                    else labelText.Color = silver;
                };
                infoText.OnUpdate += () =>
                {
                    if (!KnightInSilksong.IsKnight)
                    {
                        infoText.Color = Color.white;
                    }
                    else
                    {
                        infoText.Color = silver;
                        infoText.Text = knight_info();
                    }
                };
            }
            void KnightBool(Func<bool> knight_info) => KnightInfo(() => InfoPanel.GetStringForBool(knight_info()));
            void KnightT<T>(Func<T> knight_info) => KnightInfo(() => knight_info().ToString());
            switch (label)
            {
                case "Position":
                    KnightInfo(() =>
                    {
                        if (hc == null) return string.Empty;
                        return $"{hc.transform.position.x:.000000#}, {hc.transform.position.y:.000000#}";
                    });
                    break;
                case "Velocity":
                    KnightT(() => hc.current_velocity);
                    break;
                case "Hero State":
                    KnightT(() => hc.hero_state);
                    break;
                case "Damage State":
                    KnightT(() => hc.damageMode);
                    break;
                case "Needle Base":
                    KnightLabel("Needle Base", "Nail Base");
                    KnightInfo(() => RefKnightSlash.FsmVariables.GetFsmInt("damageDealt").Value + " (Flat " + PlayerData.instance.nailDamage + ", x" + RefKnightSlash.FsmVariables.GetFsmFloat("Multiplier").Value + ")");
                    break;
                case "Health":
                    KnightInfo(() => $"{pd.health} / {pd.maxHealth}");
                    break;
                case "Silk":
                    KnightLabel("Silk", "Soul");
                    KnightT(() => (pd.MPCharge + pd.MPReserve));
                    break;
                case "Attacking":
                    KnightBool(() => hc.cState.attacking);
                    break;
                case "Sprinting":
                    KnightLabel("Sprinting", "Dashing");
                    KnightBool(() => hc.cState.dashing || hc.cState.shadowDashing);
                    break;
                case "Jumping":
                    KnightBool(() => (hc.cState.jumping || hc.cState.doubleJumping));
                    break;
                case "Falling":
                    KnightBool(() => hc.cState.falling);
                    break;
                case "Hardland":
                    KnightBool(() => hc.cState.willHardLand);
                    break;
                case "Swimming":
                    KnightBool(() => hc.cState.swimming);
                    break;
                case "Recoiling":
                    KnightBool(() => hc.cState.recoiling);
                    break;
                case "Soaring":
                    KnightLabel("Soaring", "Superdashing");
                    KnightBool(() => hc.cState.superDashing);
                    break;
                case "Can Cast":
                    KnightBool(() => hc.CanCast());
                    break;
                case "Can Soar":
                    KnightLabel("Can Soar", "Can Superdash");
                    KnightBool(() => hc.CanSuperDash());
                    break;
                case "Can Quickmap":
                    KnightBool(() => hc.CanQuickMap());
                    break;
                case "Can Inventory":
                    KnightBool(() => hc.CanOpenInventory());
                    break;
                case "Accept Input":
                    KnightBool(() => hc.acceptingInput);
                    break;
                case "Relinquished":
                    KnightBool(() => hc.controlReqlinquished);
                    break;
                case "At Bench":
                    KnightBool(() => pd.atBench);
                    break;
                case "Invulnerable":
                    KnightBool(() => hc.cState.invulnerable);
                    break;
                case "Invincible":
                    KnightBool(() => pd.isInvincible);
                    break;
            }
        }
        #endregion

        #region  KnightPanel
        [HarmonyPostfix]
        [HarmonyPatch(typeof(MainPanel), MethodType.Constructor)]
        public static void MainPanel_Postfix(MainPanel __instance)
        {
            void AppendLeveledTile(string name, Func<int> getter, Action effect, List<Texture2D> icons, bool includeLabel = true, int defaultRowWidth = 5)
            {
                CanvasPanel leveledTile = null;
                leveledTile = __instance.AppendLabeledTile(name, () =>
                {
                    var level = getter.Invoke();
                    var iconIndex = Math.Max(0, level - 1);
                    var icon = iconIndex < icons.Count ? icons[iconIndex] : null;
                    if (icon != null)
                    {
                        // ReSharper disable once AccessToModifiedClosure
                        leveledTile!.Get<CanvasImage>("Icon")?.SetImage(icon);
                    }
                    return level > 0;
                }, effect, includeLabel: includeLabel, defaultRowWidth: defaultRowWidth);
            }
            void AppendCustomImageTitle(string name, Func<bool> getter, Action effect, Texture2D icon, bool includeLabel = true, int defaultRowWidth = 5)
            {
                CanvasPanel customImageTitle = null;
                customImageTitle = __instance.AppendLabeledTile(name, () =>
                {

                    if (icon != null)
                    {
                        customImageTitle!.Get<CanvasImage>("Icon")?.SetImage(icon);
                    }
                    return getter.Invoke();
                }, effect, includeLabel: includeLabel, defaultRowWidth: defaultRowWidth);
            }
            void AppendCustomImageTitleWithSprite(string name, Func<bool> getter, Action effect, Sprite icon, bool includeLabel = true, int defaultRowWidth = 5)
            {
                CanvasPanel customImageTitle = null;
                customImageTitle = __instance.AppendLabeledTile(name, () =>
                {

                    if (icon != null)
                    {
                        customImageTitle!.Get<CanvasImage>("Icon")?.SetImage(icon);
                    }
                    return getter.Invoke();
                }, effect, includeLabel: includeLabel, defaultRowWidth: defaultRowWidth);
            }
            void AppendCustomImageTitleByName(string name, Func<bool> getter, Action effect, string icon_name, bool includeLabel = true, int defaultRowWidth = 5)
            {
                AppendCustomImageTitle(name, getter, effect, KISHelper.ResourceTexture(icon_name), includeLabel, defaultRowWidth);
            }

            __instance.AddTab("Knight");
            __instance.AppendSectionHeader("Control");
            __instance.AppendRow(1);
            __instance.AppendBasicControl(LangKey.SWITCH_CHARACTER.InGameKey(), KnightInSilksong.Instance.ToggleKnight);
            __instance.AppendSectionHeader("Skill");
            __instance.AppendRow(1);
            __instance.AppendBasicControl(LangKey.DEBUG_ALL_SKILL.InGameKey(), GiveAllSkills);

            AppendLeveledTile(LangKey.DEBUG_SCREAM_NAME.InGameKey(), () => pd.GetInt(nameof(pd.screamLevel)), () => IncreaseSpellLevel(Spell.Scream),
                [KISHelper.ResourceTexture("inv_scream_01"), KISHelper.ResourceTexture("inv_scream_02")]);
            AppendLeveledTile(LangKey.DEBUG_FIREBALL_NAME.InGameKey(), () => pd.GetInt(nameof(pd.fireballLevel)), () => IncreaseSpellLevel(Spell.Fireball),
                [KISHelper.ResourceTexture("inv_fireball_01"), KISHelper.ResourceTexture("inv_fireball_02")]);
            AppendLeveledTile(LangKey.DEBUG_QUAKE_NAME.InGameKey(), () => pd.GetInt(nameof(pd.quakeLevel)), () => IncreaseSpellLevel(Spell.Quake),
                [KISHelper.ResourceTexture("inv_quake_01"), KISHelper.ResourceTexture("inv_quake_02")]);

            AppendLeveledTile(LangKey.DEBUG_DASH_NAME.InGameKey(), GetDashLevel, CycleDash,
                [KISHelper.ResourceTexture("inv_dash_cloak"), KISHelper.ResourceTexture("inv_shade_cloak")]);
            AppendCustomImageTitleByName(LangKey.DEBUG_WALL_JUMP_NAME.InGameKey(),
                                            () => pd.hasWalljump,
                                            ToggleWallJump,
                                            "inv_mantis_claw");
            AppendCustomImageTitleByName(LangKey.DEBUG_DOUBLE_JUMP_NAME.InGameKey(),
                                            () => pd.GetBool(nameof(pd.hasDoubleJump)),
                                            ToggleDoubleJump,
                                            "inv_emperor_wings");
            AppendLeveledTile(LangKey.DEBUG_DREAM_NAIL_NAME.InGameKey(), GetDreamNailLevel, CycleDreamNail,
                [KISHelper.ResourceTexture("inv_dream_nail"), KISHelper.ResourceTexture("inv_dream_nail_upgraded")]);
            AppendCustomImageTitleByName(LangKey.DEBUG_DREAM_GATE_NAME.InGameKey(),
                                            () => pd.GetBool(nameof(pd.hasDreamGate)),
                                            ToggleDreamGate,
                                            "inv_dream_gate");
            AppendCustomImageTitleByName(LangKey.DEBUG_SUPER_DASH_NAME.InGameKey(),
                                            () => pd.GetBool(nameof(pd.hasSuperDash)),
                                            ToggleSuperDash,
                                            "inv_crystal_heart");
            AppendCustomImageTitleByName(LangKey.DEBUG_ACID_SWIM_NAME.InGameKey(),
                                            () => pd.GetBool(nameof(pd.hasAcidArmour)),
                                            ToggleAcidSwim,
                                            "inv_acid_armour");
            AppendCustomImageTitleByName(LangKey.DEBUG_GREAT_SLASH_NAME.InGameKey(),
                                            () => pd.GetBool(nameof(pd.hasDashSlash)),
                                            () => ToggleNailArt(NailArt.GREAT_SLASH),
                                            "inv_upgraded_slash");
            AppendCustomImageTitleByName(LangKey.DEBUG_DASH_SLASH_NAME.InGameKey(),
                                            () => pd.GetBool(nameof(pd.hasUpwardSlash)),
                                            () => ToggleNailArt(NailArt.DASH_SLASH),
                                            "inv_dash_slash");
            AppendCustomImageTitleByName(LangKey.DEBUG_CYCLONE_NAME.InGameKey(),
                                            () => pd.GetBool(nameof(pd.hasCyclone)),
                                            () => ToggleNailArt(NailArt.CYCLONE),
                                            "inv_cyclone");
            __instance.AppendSectionHeader("Charm");
            __instance.AppendRow(1, 1);
            __instance.AppendBasicControl(LangKey.DEBUG_ALL_CHARMS.InGameKey(),
                                            GiveAllCharms);
            __instance.AppendBasicControl(LangKey.DEBUG_REMOVE_ALL_CHARMS.InGameKey(),
                RemoveAllCharms);
            __instance.AppendRow(1);
            __instance.AppendNumericControl(LangKey.CHARMSLOT_NAME.InGameKey(),
                                            () => pd.charmSlots,
                                            3,
                                            (val) => pd.SetInt(nameof(pd.charmSlots), (int)val),
                                            IncreaseCharmSlots,
                                            DecreaseCharmSlots,
                                            () => pd.SetInt(nameof(pd.charmSlots), 3));
            __instance.AppendRow(1);
            __instance.AppendBasicControl(LangKey.DEBUG_FIX_CHARMSLOTS.InGameKey(), FixCharmSlots);
            //The assignment of `KnightInSilksong.Instance.charm` is earlier than here so it's OK.
            CharmIconList charmIconList = KnightInSilksong.Instance.charm.FindGameObjectInChildren("Charm Icons").GetComponent<CharmIconList>();
            for (int i = 1; i <= 40; i++)
            {
                if (i % 5 == 1) __instance.AppendTileRow(5);
                Charm charm = (Charm)i;
                if (charm.IsFragileCharm())
                {
                    string name = charm.ToString().Replace("Unbreakable", "").ToLower();
                    AppendLeveledTile(charm.InGameKey(),
                                                () => GetFragileState(charm),
                                                () => CycleCharm(charm),
                                                [KISHelper.ResourceTexture("charm_glass_"+name), KISHelper.ResourceTexture("charm_broken_"+name),
                                                    KISHelper.ResourceTexture("charm_full_"+name)]);
                    continue;
                }
                switch (charm)
                {
                    case Charm.Grimmchild:
                        AppendLeveledTile(LangKey.GRIMM_CHILD.InGameKey(),
                                                () => !pd.gotCharm_40 ? 0 : pd.grimmChildLevel,
                                                () => CycleCharm(charm),
                                                [KISHelper.ResourceTexture("charm_grimmkin_01"),
                                                KISHelper.ResourceTexture("charm_grimmkin_02"),
                                                KISHelper.ResourceTexture("charm_grimmkin_03"),
                                                KISHelper.ResourceTexture("charm_grimmkin_04"),
                                                KISHelper.ResourceTexture("charm_grimmkin_05")]);
                        break;
                    case Charm.VoidHeart:
                        AppendLeveledTile(LangKey.KING_SOUL.InGameKey(),
                                                () => !pd.gotCharm_36 ? 0 : pd.royalCharmState,
                                                () => CycleCharm(Charm.VoidHeart),
                                                [KISHelper.ResourceTexture("charm_white_left"), KISHelper.ResourceTexture("charm_white_right"),
                                                    KISHelper.ResourceTexture("charm_white_full"), KISHelper.ResourceTexture("charm_black")]);
                        break;
                    default:
                        AppendCustomImageTitleWithSprite(charm.InGameKey(),
                                                            () => pd.GetBool("gotCharm_" + (int)charm),
                                                            () => CycleCharm(charm),
                                                            charmIconList.GetSprite(i));
                        break;
                }
            }
            var text = __instance.AppendSectionHeader(LangKey.DEBUG_PROMPT.InGameKey());
            text.FontSize = MainPanel.KeybindHeaderFontSize;

        }
        private static void RegisterBindableFunctions()
        {
            DebugMod.DebugMod.AddActionToKeyBindList(GiveAllSkills, LangKey.DEBUG_ALL_SKILL.InGameKey(), "KNIGHT");
            DebugMod.DebugMod.AddActionToKeyBindList(GiveAllCharms, LangKey.DEBUG_ALL_CHARMS.InGameKey(), "KNIGHT");
            DebugMod.DebugMod.AddActionToKeyBindList(RemoveAllCharms, LangKey.DEBUG_REMOVE_ALL_CHARMS.InGameKey(), "KNIGHT");
            DebugMod.DebugMod.AddActionToKeyBindList(IncreaseCharmSlots, LangKey.DEBUG_INCREASE_CHARMSLOTS.InGameKey(), "KNIGHT");
            DebugMod.DebugMod.AddActionToKeyBindList(DecreaseCharmSlots, LangKey.DEBUG_DECREASE_CHARMSLOTS.InGameKey(), "KNIGHT");
            DebugMod.DebugMod.AddActionToKeyBindList(FixCharmSlots, LangKey.DEBUG_FIX_CHARMSLOTS.InGameKey(), "KNIGHT");
        }
        private static void UpdateCharmsEffects()
        {
            PlayMakerFSM.BroadcastEvent("CHARM INDICATOR CHECK");
            PlayMakerFSM.BroadcastEvent("CHARM EQUIP CHECK");

            IEnumerator SpawnGrimmChild()
            {
                for (int i = 0; i < 2; i++) yield return null;
                hc?.transform.Find("Charm Effects").gameObject.LocateMyFSM("Spawn Grimmchild").SendEvent("CHARM EQUIP CHECK");
            }

            Object.Destroy(GameObject.FindWithTag("Grimmchild"));
            if (pd.equippedCharm_40) // Grimmchild
            {
                GameManager.instance.StartCoroutine(SpawnGrimmChild());
            }
        }

        public static void ToggleNailArt(NailArt nailArt)
        {
            var name = nailArt.PdGotField();
            pd.SetBool(name, !pd.GetBool(name));
            pd.SetBool(nameof(pd.hasNailArt), pd.GetBool(nameof(pd.hasDashSlash)) || pd.GetBool(nameof(pd.hasUpwardSlash)) || pd.GetBool(nameof(pd.hasCyclone)));
            pd.SetBool(nameof(pd.hasAllNailArts), pd.GetBool(nameof(pd.hasDashSlash)) && pd.GetBool(nameof(pd.hasUpwardSlash)) && pd.GetBool(nameof(pd.hasCyclone)));
        }
        public static void ToggleAcidSwim()
        {
            var prior = pd.GetBool(nameof(pd.hasAcidArmour));
            pd.SetBool(nameof(pd.hasAcidArmour), !prior);
            if (!prior) PlayMakerFSM.BroadcastEvent("GET ACID ARMOUR");
        }
        public static void ToggleSuperDash()
        {
            var prior = pd.GetBool(nameof(pd.hasSuperDash));
            pd.SetBool(nameof(pd.hasSuperDash), !prior);
            pd.SetBool(nameof(pd.canSuperDash), !prior);
        }
        public static void ToggleDreamGate() // TODO: should dreamgate be rolled into Dream Nail level?
        {
            if (!pd.GetBool(nameof(pd.hasDreamGate)))
            {
                if (!pd.GetBool(nameof(pd.hasDreamNail))) pd.SetBool(nameof(pd.hasDreamNail), true);
                pd.SetBool(nameof(pd.hasDreamGate), true);
                hc?.gameObject.LocateMyFSM("Dream Nail").FsmVariables.FindFsmBool("Dream Warp Allowed").Value = true;
            }
            else
            {
                pd.SetBool(nameof(pd.hasDreamGate), false);
                hc?.gameObject.LocateMyFSM("Dream Nail").FsmVariables.FindFsmBool("Dream Warp Allowed").Value = false;
            }
        }
        public static int GetDreamNailLevel()
        {
            if (pd.GetBool(nameof(pd.hasDreamNail)) && pd.GetBool(nameof(pd.dreamNailUpgraded))) return 2;
            if (pd.GetBool(nameof(pd.hasDreamNail))) return 1;
            return 0;
        }
        public static void CycleDreamNail()
        {
            var nextState = (GetDreamNailLevel() + 1) % 3; // range 0-2
            pd.SetBool(nameof(pd.hasDreamNail), nextState > 0);
            pd.SetBool(nameof(pd.dreamNailUpgraded), nextState == 2);
        }
        public static void ToggleDoubleJump()
        {
            pd.SetBool(nameof(pd.hasDoubleJump), !pd.GetBool(nameof(pd.hasDoubleJump)));
        }
        public static void ToggleWallJump()
        {
            var prior = pd.GetBool(nameof(pd.hasWalljump));
            pd.SetBool(nameof(pd.hasWalljump), !prior);
            pd.SetBool(nameof(pd.canWallJump), !prior);
        }
        public static int GetDashLevel()
        {
            return !pd.GetBool(nameof(pd.hasDash)) ? 0 :
                pd.GetBool(nameof(pd.hasShadowDash)) ? 2 : 1;
        }
        public static void CycleDash()
        {
            var nextLevel = (GetDashLevel() + 1) % 3; // range 0-2
            pd.SetBool(nameof(pd.hasDash), nextLevel > 0);
            pd.SetBool(nameof(pd.canDash), nextLevel > 0);
            pd.SetBool(nameof(pd.hasShadowDash), nextLevel == 2);
            pd.SetBool(nameof(pd.canShadowDash), nextLevel == 2);
            if (nextLevel == 2) EventRegister.SendEvent("GOT SHADOW DASH");
        }
        public static void GiveAllSkills()
        {
            pd?.screamLevel = 2;
            pd?.fireballLevel = 2;
            pd?.quakeLevel = 2;

            pd?.hasDash = true;
            pd?.canDash = true;
            pd?.hasShadowDash = true;
            pd?.canShadowDash = true;
            pd?.hasWalljump = true;
            pd?.canWallJump = true;
            pd?.hasDoubleJump = true;
            pd?.hasSuperDash = true;
            pd?.canSuperDash = true;
            pd?.hasAcidArmour = true;

            pd?.hasDreamNail = true;
            pd?.dreamNailUpgraded = true;
            pd?.hasDreamGate = true;

            pd?.hasNailArt = true;
            pd?.hasCyclone = true;
            pd?.hasDashSlash = true;
            pd?.hasUpwardSlash = true;
            pd?.hasAllNailArts = true;
        }
        public static void IncreaseCharmSlots()
        {
            pd.IncrementInt(nameof(pd.charmSlots));
        }
        public static void DecreaseCharmSlots()
        {
            pd.DecrementInt(nameof(pd.charmSlots));
        }
        public static void GiveAllCharms()
        {
            for (int i = 1; i <= 40; i++)
            {
                pd.SetBool("gotCharm_" + i, true);
            }
            pd.SetInt(nameof(pd.charmSlots), 10);
            pd.SetBool(nameof(pd.hasCharm), true);
            pd.SetInt(nameof(pd.charmsOwned), 40);
            pd.SetInt(nameof(pd.royalCharmState), 4);
            pd.SetBool(nameof(pd.gotShadeCharm), true);
            pd.SetInt(nameof(pd.charmCost_36), 0);
            pd.SetBool(nameof(pd.fragileGreed_unbreakable), true);
            pd.SetBool(nameof(pd.fragileStrength_unbreakable), true);
            pd.SetBool(nameof(pd.fragileHealth_unbreakable), true);
            pd.SetInt(nameof(pd.grimmChildLevel), 5);
            pd.SetInt(nameof(pd.charmCost_40), 3);
            pd.SetInt(nameof(pd.charmSlots), 11);
            UpdateCharmsEffects();
        }
        public static void RemoveAllCharms()
        {
            for (int i = 1; i <= 40; i++)
            {
                pd.SetBool("gotCharm_" + i, false);
                pd.SetBool("equippedCharm_" + i, false);
            }

            pd.SetInt(nameof(pd.charmSlots), 3);
            pd.SetBool(nameof(pd.hasCharm), false);
            pd.SetInt(nameof(pd.charmsOwned), 0);
            pd.SetInt(nameof(pd.royalCharmState), 0);
            pd.SetBool(nameof(pd.gotShadeCharm), false);
            pd.SetBool(nameof(pd.fragileGreed_unbreakable), true);
            pd.SetBool(nameof(pd.fragileStrength_unbreakable), true);
            pd.SetBool(nameof(pd.fragileHealth_unbreakable), true);
            pd.SetInt(nameof(pd.grimmChildLevel), 0);
            pd.SetInt(nameof(pd.charmCost_40), 2);
            pd.SetInt(nameof(pd.charmSlots), 3);
            pd.equippedCharms.Clear();
            pd.SetInt(nameof(pd.charmSlotsFilled), 0);
            UpdateCharmsEffects();
        }
        public static void FixCharmSlots()
        {
            pd.CalculateNotchesUsed();
        }

        public static void CycleCharm(int charmId) => CycleCharm((Charm)charmId);
        public static void CycleCharm(Charm charm)
        {
            var charmId = (int)charm;
            var hasCharm = pd.GetBool($"gotCharm_{charmId}");

            switch (charmId)
            {
                case 40: // Grimmchild
                    var newChildLevel = (pd.grimmChildLevel + 1) % 6; // Range 0-5
                    pd.SetBool(nameof(pd.gotCharm_40), newChildLevel > 0);
                    pd.SetInt(nameof(pd.charmCost_40), newChildLevel == 5 ? 3 : 2);
                    pd.SetInt(nameof(pd.grimmChildLevel), newChildLevel);
                    pd.SetBool(nameof(pd.destroyedNightmareLantern), newChildLevel == 5);
                    break;
                case 36: // Kingsoul
                    var newKsLevel = (pd.royalCharmState + 1) % 5; // Range 0-4
                    pd.gotCharm_36 = newKsLevel > 0;
                    pd.royalCharmState = newKsLevel;
                    pd.charmCost_36 = newKsLevel switch
                    {
                        3 => 5,
                        4 => 0,
                        _ => pd.charmCost_36
                    };
                    break;
                case 23:
                case 24:
                case 25: // Fragile charms
                    var nextState = (GetFragileState(charm) + 1) % 4;  // range 0-3
                    pd.SetBool(charm.PdGotField(), nextState != 0);
                    pd.SetBool(charm.PdUnbreakableField(), nextState == 3);
                    pd.SetBool(charm.PdBrokenField(), nextState == 2);
                    break;
                default:
                    pd.SetBool($"gotCharm_{charmId}", !hasCharm);
                    break;
            }
            UpdateCharmsEffects();
        }

        public static int GetFragileState(Charm charm)
        {
            if (!pd.GetBool(charm.PdGotField())) return 0; // unobtained
            if (pd.GetBool(charm.PdUnbreakableField())) return 3; // unbreakable
            return pd.GetBool(charm.PdBrokenField()) ? 2 : 1; // broken / unbroken
        }
        public static void IncreaseSpellLevel(Spell spell)
        {
            var name = spell switch
            {
                Spell.Fireball => nameof(pd.fireballLevel),
                Spell.Quake => nameof(pd.quakeLevel),
                Spell.Scream => nameof(pd.screamLevel),
                _ => throw new ArgumentOutOfRangeException(nameof(spell), spell, "Not a valid spell")
            };
            pd.SetInt(name, (pd.GetInt(name) + 1) % 3); // range 0-2
        }
        #endregion
    }
}