using System.Collections;
using DebugMod;
using DebugMod.UI;
using DebugMod.UI.Canvas;
using TeamCherry.Localization;
using UnityEngine.UI;
using KIS.Utils;
namespace KIS.Compatibility
{
    [RequiresMod(DebugMod.DebugMod.Id)]
    public class WithDebugMod : ICompatibility
    {
        public string ModName => "Debug Mod";

        public string ModId => DebugMod.DebugMod.Id;

        public DebugMod.DebugMod debug => DebugMod.DebugMod.instance;
        static Knight.PlayerData pd => Knight.PlayerData.instance;
        static Knight.HeroController hc => Knight.HeroController.instance;
        private static PlayMakerFSM _refKnightSlash;
        internal static PlayMakerFSM RefKnightSlash => _refKnightSlash != null ? _refKnightSlash : (_refKnightSlash = hc?.transform.Find("Attacks/Slash").GetComponent<PlayMakerFSM>());
        public static bool infinite_jump = false;


        public void Init()
        {
            DebugMod.DebugMod.Log("Debug Mod detected KIS, initializing compatibility.");
            KnightInSilksong.Instance.self_hormony.PatchAll(typeof(WithDebugMod));

        }

        public void Update()
        {
            if (!KnightInSilksong.IsKnight) return;
            if (DebugMod.DebugMod.noclip)
            {
                hc?.transform.position = DebugMod.DebugMod.noclipPos;
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
        #region Replace Functions
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Knight.PlayerData), nameof(Knight.PlayerData.TakeHealth), MethodType.Normal)]
        public static bool Debug_Knight_PlayerData_TakeHealth_Prefix(Knight.PlayerData __instance, ref int amount)
        {
            if (!KnightInSilksong.IsKnight) return true;
            DebugMod.ModHooks.PlayerData_TakeHealth(ref amount);
            return true;
        }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(DebugMod.SaveStates.SaveState), nameof(DebugMod.SaveStates.SaveState.Load), MethodType.Enumerator)]
        public static void DebugMod_SaveStates_SaveState_Load_Postfix(DebugMod.SaveStates.SaveState __instance)
        {
            if (!KnightInSilksong.IsKnight) return;
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
        [HarmonyPatch(typeof(InfoPanel), MethodType.Constructor)]
        public static void InfoPanel__Postfix(InfoPanel __instance)
        {
            __instance.AppendInfo("Character", () => KnightInSilksong.IsKnight ? "Knight" : "Hornet");
        }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(InfoPanel), nameof(InfoPanel.AppendInfo), [typeof(string), typeof(Func<string>)])]
        public static void InfoPanel_AppendInfo_Postfix(InfoPanel __instance, string label, ref Func<string> info)
        {
            Color silver = new(192f / 255, 192f / 255, 192f / 255);
            int counter = __instance.counter - 1;
            CanvasText labelText = (CanvasText)__instance.elements[$"Label{counter}"];
            CanvasText infoText = (CanvasText)__instance.elements[$"Info{counter}"];
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
                        return;
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
                case "Character":
                    KnightInfo(() => KnightInSilksong.IsKnight ? "Knight" : "Hornet");
                    break;
                default:
                    break;
            }
        }
        #endregion
        #region  KnightPanel
        private static void UpdateCharmsEffects()
        {
            PlayMakerFSM.BroadcastEvent("CHARM INDICATOR CHECK");
            PlayMakerFSM.BroadcastEvent("CHARM EQUIP CHECK");
        }
        private static void UpdateNailArtStates()
        {
            pd.SetBool(nameof(pd.hasNailArt),
                        pd.GetBool(nameof(pd.hasDashSlash)) || pd.GetBool(nameof(pd.hasUpwardSlash)) || pd.GetBool(nameof(pd.hasCyclone)));
            pd.SetBool(nameof(pd.hasAllNailArts),
                        pd.GetBool(nameof(pd.hasDashSlash)) && pd.GetBool(nameof(pd.hasUpwardSlash)) && pd.GetBool(nameof(pd.hasCyclone)));
        }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(MainPanel), MethodType.Constructor)]
        public static void MainPanel_Postfix(MainPanel __instance)
        {
            __instance.AddTab("Knight");
            __instance.AppendSectionHeader("Skill");
            __instance.AppendRow(1);
            __instance.AppendBasicControl(LangKey.DEBUG_ALL_SKILL.Localize(), GiveAllSkills);
            __instance.AppendRow(1, 1, 1);
            __instance.AppendIncrementControl(LangKey.DEBUG_SCREAM_NAME.Localize(),
                                                () => pd.GetInt(nameof(pd.screamLevel)),
                                                () => IncreaseSpellLevel(Spell.Scream));
            __instance.AppendIncrementControl(LangKey.DEBUG_FIREBALL_NAME.Localize(),
                                                () => pd.GetInt(nameof(pd.fireballLevel)),
                                                () => IncreaseSpellLevel(Spell.Fireball));
            __instance.AppendIncrementControl(LangKey.DEBUG_QUAKE_NAME.Localize(),
                                                () => pd.GetInt(nameof(pd.quakeLevel)),
                                                () => IncreaseSpellLevel(Spell.Quake));
            __instance.AppendRow(1, 1, 1);
            __instance.AppendIncrementControl(LangKey.DEBUG_DASH_NAME.Localize(), GetDashLevel, ToggleDash);
            __instance.AppendToggleControl(LangKey.DEBUG_WALL_JUMP_NAME.Localize(),
                                            () => pd.GetBool(nameof(pd.hasWalljump)),
                                            ToggleWallJump);
            __instance.AppendToggleControl(LangKey.DEBUG_DOUBLE_JUMP_NAME.Localize(),
                                            () => pd.GetBool(nameof(pd.hasDoubleJump)),
                                            ToggleDoubleJump);
            __instance.AppendRow(1, 1);
            __instance.AppendIncrementControl(LangKey.DEBUG_DREAM_NAIL_NAME.Localize(), GetDreamNailLevel, ToggleDreamNail);
            __instance.AppendToggleControl(LangKey.DEBUG_DREAM_GATE_NAME.Localize(),
                                            () => pd.GetBool(nameof(pd.hasDreamGate)),
                                            ToggleDreamGate);
            __instance.AppendRow(1, 1);
            __instance.AppendToggleControl(LangKey.DEBUG_SUPER_DASH_NAME.Localize(),
                                            () => pd.GetBool(nameof(pd.hasSuperDash)),
                                            ToggleSuperDash);
            __instance.AppendToggleControl(LangKey.DEBUG_ACID_SWIM_NAME.Localize(),
                                            () => pd.GetBool(nameof(pd.hasAcidArmour)),
                                            ToggleAcidSwim);
            __instance.AppendRow(1, 1, 1);
            __instance.AppendToggleControl(LangKey.DEBUG_GREAT_SLASH_NAME.Localize(),
                                            () => pd.GetBool(nameof(pd.hasDashSlash)),
                                            () => ToggleNailArt(NailArt.GREAT_SLASH));
            __instance.AppendToggleControl(LangKey.DEBUG_DASH_SLASH_NAME.Localize(),
                                            () => pd.GetBool(nameof(pd.hasUpwardSlash)),
                                            () => ToggleNailArt(NailArt.DASH_SLASH));
            __instance.AppendToggleControl(LangKey.DEBUG_CYCLONE_NAME.Localize(),
                                            () => pd.GetBool(nameof(pd.hasCyclone)),
                                            () => ToggleNailArt(NailArt.CYCLONE));
            __instance.AppendSectionHeader("Charm");
            __instance.AppendRow(1);
            __instance.AppendBasicControl(LangKey.DEBUG_ALL_CHARMS.Localize(),
                                            GiveAllCharms);
            __instance.AppendRow(1, 1);
            __instance.AppendIncrementControl(LangKey.KING_SOUL.Localize(),
                                                () => pd.GetInt(nameof(pd.royalCharmState)),
                                                IncreaseRoyalGameState);
            __instance.AppendIncrementControl(LangKey.GRIMM_CHILD.Localize(),
                                                () => pd.GetInt(nameof(pd.grimmChildLevel)),
                                                IncreaseGrimmChildLevel);
            __instance.AppendRow(1, 1);
            __instance.AppendBasicControl(LangKey.DEBUG_INCREASE_CHARMSLOTS.Localize(),
                                                IncreaseCharmSlots);
            __instance.AppendBasicControl(LangKey.DEBUG_DECREASE_CHARMSLOTS.Localize(),
                                                DecreaseCharmSlots);
            __instance.AppendRow(1);
            __instance.AppendBasicControl(LangKey.DEBUG_REMOVE_ALL_CHARMS.Localize(),
                                            RemoveAllCharms);
            __instance.AppendRow(1);
            __instance.AppendRow(1);
            var text = __instance.AppendSectionHeader(LangKey.DEBUG_PROMPT.Localize());
            text.FontSize = MainPanel.KeybindHeaderFontSize;

        }
        public static void ToggleNailArt(NailArt nailArt)
        {
            //yes, that's how tc did.
            string name = nailArt switch
            {
                NailArt.GREAT_SLASH => nameof(pd.hasDashSlash),
                NailArt.DASH_SLASH => nameof(pd.hasUpwardSlash),
                NailArt.CYCLONE => nameof(pd.hasCyclone),
                _ => null
            };
            pd.SetBool(name, !pd.GetBool(name));
            UpdateNailArtStates();
            return;
        }
        public static void ToggleAcidSwim()
        {
            if (!pd.GetBool(nameof(pd.hasAcidArmour)))
            {
                pd.SetBool(nameof(pd.hasAcidArmour), true);
                PlayMakerFSM.BroadcastEvent("GET ACID ARMOUR");
            }
            else
            {
                pd.SetBool(nameof(pd.hasAcidArmour), false);
            }
        }
        public static void ToggleSuperDash()
        {
            if (!pd.GetBool(nameof(pd.hasSuperDash)))
            {
                pd.SetBool(nameof(pd.hasSuperDash), true);
                pd.SetBool(nameof(pd.canSuperDash), true);
            }
            else
            {
                pd.SetBool(nameof(pd.hasSuperDash), false);
                pd.SetBool(nameof(pd.canSuperDash), false);
            }
        }
        public static void ToggleDreamGate()
        {
            if (!pd.GetBool(nameof(pd.hasDreamNail)) && !pd.GetBool(nameof(pd.hasDreamGate)))
            {
                pd.SetBool(nameof(pd.hasDreamNail), true);
                pd.SetBool(nameof(pd.hasDreamGate), true);
                hc?.gameObject.LocateMyFSM("Dream Nail").FsmVariables.FindFsmBool("Dream Warp Allowed").Value = true;
            }
            else if (pd.GetBool(nameof(pd.hasDreamNail)) && !pd.GetBool(nameof(pd.hasDreamGate)))
            {
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
        public static void ToggleDreamNail()
        {
            if (!pd.GetBool(nameof(pd.hasDreamNail)) && !pd.GetBool(nameof(pd.dreamNailUpgraded)))
            {
                pd.SetBool(nameof(pd.hasDreamNail), true);
            }
            else if (pd.GetBool(nameof(pd.hasDreamNail)) && !pd.GetBool(nameof(pd.dreamNailUpgraded)))
            {
                pd.SetBool(nameof(pd.dreamNailUpgraded), true);
            }
            else
            {
                pd.SetBool(nameof(pd.hasDreamNail), false);
                pd.SetBool(nameof(pd.dreamNailUpgraded), false);
            }
        }
        public static void ToggleDoubleJump()
        {
            if (!pd.GetBool(nameof(pd.hasDoubleJump)))
            {
                pd.SetBool(nameof(pd.hasDoubleJump), true);
            }
            else
            {
                pd.SetBool(nameof(pd.hasDoubleJump), false);
            }
        }
        public static void ToggleWallJump()
        {
            if (!pd.GetBool(nameof(pd.hasWalljump)))
            {
                pd.SetBool(nameof(pd.hasWalljump), true);
                pd.SetBool(nameof(pd.canWallJump), true);
            }
            else
            {
                pd.SetBool(nameof(pd.hasWalljump), false);
                pd.SetBool(nameof(pd.canWallJump), false);
            }
        }
        public static int GetDashLevel()
        {
            if (pd.GetBool(nameof(pd.hasDash)) && pd.GetBool(nameof(pd.hasShadowDash))) return 2;
            if (pd.GetBool(nameof(pd.hasDash))) return 1;
            return 0;
        }
        public static void ToggleDash()
        {
            if (!pd.GetBool(nameof(pd.hasDash)) && !pd.GetBool(nameof(pd.hasShadowDash)))
            {
                pd.SetBool(nameof(pd.hasDash), true);
                pd.SetBool(nameof(pd.canDash), true);
            }
            else if (pd.GetBool(nameof(pd.hasDash)) && !pd.GetBool(nameof(pd.hasShadowDash)))
            {
                pd.SetBool(nameof(pd.hasShadowDash), true);
                pd.SetBool(nameof(pd.canShadowDash), true);
                EventRegister.SendEvent("GOT SHADOW DASH");
            }
            else
            {
                pd.SetBool(nameof(pd.hasDash), false);
                pd.SetBool(nameof(pd.canDash), false);
                pd.SetBool(nameof(pd.hasShadowDash), false);
                pd.SetBool(nameof(pd.canShadowDash), false);
            }
        }
        public static void GiveAllSkills()
        {
            pd.SetInt(nameof(pd.screamLevel), 2);
            pd.SetInt(nameof(pd.fireballLevel), 2);
            pd.SetInt(nameof(pd.quakeLevel), 2);
            pd.SetBool(nameof(pd.hasDash), true);
            pd.SetBool(nameof(pd.canDash), true);
            pd.SetBool(nameof(pd.hasShadowDash), true);
            pd.SetBool(nameof(pd.canShadowDash), true);
            pd.SetBool(nameof(pd.hasWalljump), true);
            pd.SetBool(nameof(pd.canWallJump), true);
            pd.SetBool(nameof(pd.hasDoubleJump), true);
            pd.SetBool(nameof(pd.hasSuperDash), true);
            pd.SetBool(nameof(pd.canSuperDash), true);
            pd.SetBool(nameof(pd.hasAcidArmour), true);
            pd.SetBool(nameof(pd.hasDreamNail), true);
            pd.SetBool(nameof(pd.dreamNailUpgraded), true);
            pd.SetBool(nameof(pd.hasDreamGate), true);
            pd.SetBool(nameof(pd.hasNailArt), true);
            pd.SetBool(nameof(pd.hasCyclone), true);
            pd.SetBool(nameof(pd.hasDashSlash), true);
            pd.SetBool(nameof(pd.hasUpwardSlash), true);
            pd.SetBool(nameof(pd.hasAllNailArts), true);
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
            pd.SetInt(nameof(pd.grimmChildLevel), 5);
            pd.SetInt(nameof(pd.charmCost_40), 2);
            pd.SetInt(nameof(pd.charmSlots), 3);
            pd.equippedCharms.Clear();
            pd.SetInt(nameof(pd.charmSlotsFilled), 0);
            UpdateCharmsEffects();
        }
        public static void IncreaseGrimmChildLevel()
        {
            if (pd.GetBool(nameof(pd.gotCharm_40)))
            {
                pd.SetBool(nameof(pd.gotCharm_40), true);
            }
            int level = pd.GetInt(nameof(pd.grimmChildLevel));
            level += 1;
            if (level >= 6) level = 0;
            int cost = level == 5 ? 3 : 2;
            pd.SetInt(nameof(pd.grimmChildLevel), level);
            pd.SetInt(nameof(pd.charmCost_40), cost);
            pd.SetBool(nameof(pd.destroyedNightmareLantern), level == 5);
            Object.Destroy(GameObject.FindWithTag("Grimmchild"));
            UpdateCharmsEffects();
            GameManager.instance.StartCoroutine(SpawnGrimmChild());

            IEnumerator SpawnGrimmChild()
            {
                for (int i = 0; i < 2; i++) yield return null;
                hc?.transform.Find("Charm Effects").gameObject.LocateMyFSM("Spawn Grimmchild").SendEvent("CHARM EQUIP CHECK");
            }
        }
        public static void IncreaseRoyalGameState()
        {
            if (!pd.GetBool(nameof(pd.gotCharm_36)))
            {
                pd.SetBool(nameof(pd.gotCharm_36), true);
            }
            string name = nameof(pd.royalCharmState);
            int num = pd.GetInt(name);
            if (num < 4)
            {
                pd.SetInt(name, num + 1);
                num = num + 1;
            }
            else
            {
                pd.SetInt(name, 1);
                num = 1;
            }
            int cost = num switch
            {
                3 => 5,
                4 => 0,
                _ => pd.GetInt(nameof(pd.charmCost_36))
            };
            pd.SetInt(nameof(pd.gotCharm_36), cost);
        }
        public static void IncreaseSpellLevel(Spell spell)
        {
            string name = spell switch
            {
                Spell.Fireball => nameof(pd.fireballLevel),
                Spell.Quake => nameof(pd.quakeLevel),
                Spell.Scream => nameof(pd.screamLevel),
                _ => null
            };
            if (name == null) return;
            int num = pd.GetInt(name);
            if (num < 2) pd.SetInt(name, num + 1);
            else pd.SetInt(name, 0);
        }
        #endregion
    }


}