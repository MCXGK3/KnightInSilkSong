using DebugMod;
using DebugMod.UI;
using DebugMod.UI.Canvas;
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
    }


}