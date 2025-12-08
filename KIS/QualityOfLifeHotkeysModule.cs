// File: QualityOfLifeHotkeysModule.cs
using BepInEx.Configuration;
using UnityEngine;
using KIS;

namespace KIS
{
    public class QualityOfLifeHotkeysModule : IModule
    {
        private GameObject hostGO;
        private QualityOfLifeHotkeysBehaviour behaviour;

        // Hotkey config entries
        private ConfigEntry<KeyCode> KeyCycleHealth;
        private ConfigEntry<KeyCode> KeySetNailDamage;
        private ConfigEntry<KeyCode> KeyToggleDash;
        private ConfigEntry<KeyCode> KeyToggleWallJump;
        private ConfigEntry<KeyCode> KeyToggleSuperDash;
        private ConfigEntry<KeyCode> KeyToggleShadowDash;
        private ConfigEntry<KeyCode> KeyToggleDoubleJump;
        private ConfigEntry<KeyCode> KeyToggleCyclone;
        private ConfigEntry<KeyCode> KeyToggleDashSlash;
        private ConfigEntry<KeyCode> KeyToggleDreamNail;
        private ConfigEntry<KeyCode> KeyToggleDreamGate;
        private ConfigEntry<KeyCode> KeyCycleFireball;
        private ConfigEntry<KeyCode> KeyCycleQuake;
        private ConfigEntry<KeyCode> KeyCycleScream;

        public override void Init()
        {
            var cfg = KnightInSilksong.Instance.Config;

            // Bind to number/letter keys
            KeyToggleDash = cfg.Bind("Abilities", "ToggleDash", KeyCode.Alpha1, "Toggle dash ability");
            KeyToggleWallJump = cfg.Bind("Abilities", "ToggleWallJump", KeyCode.Alpha2, "Toggle wall jump ability");
            KeyToggleDoubleJump = cfg.Bind("Abilities", "ToggleDoubleJump", KeyCode.Alpha3, "Toggle double jump ability");
            KeyToggleSuperDash = cfg.Bind("Abilities", "ToggleSuperDash", KeyCode.Alpha4, "Toggle super dash ability");
            KeyToggleShadowDash = cfg.Bind("Abilities", "ToggleShadowDash", KeyCode.Alpha5, "Toggle shadow dash ability");
            KeyToggleCyclone = cfg.Bind("Abilities", "ToggleCyclone", KeyCode.Alpha6, "Toggle cyclone slash ability");
            KeyToggleDashSlash = cfg.Bind("Abilities", "ToggleDashSlash", KeyCode.Alpha7, "Toggle dash slash ability");
            KeyToggleDreamNail = cfg.Bind("Abilities", "ToggleDreamNail", KeyCode.Alpha8, "Toggle dream nail ability");
            KeyToggleDreamGate = cfg.Bind("Abilities", "ToggleDreamGate", KeyCode.Alpha9, "Toggle dream gate ability");

            KeyCycleHealth = cfg.Bind("Abilities", "CycleHealth", KeyCode.Alpha0, "Cycle health between 5–9");
            KeySetNailDamage = cfg.Bind("Abilities", "SetNailDamage", KeyCode.H, "Cycle nail damage (5,9,13,17,21)");
            KeyCycleFireball = cfg.Bind("Abilities", "CycleFireball", KeyCode.J, "Cycle fireball level");
            KeyCycleQuake = cfg.Bind("Abilities", "CycleQuake", KeyCode.K, "Cycle quake level");
            KeyCycleScream = cfg.Bind("Abilities", "CycleScream", KeyCode.L, "Cycle scream level");

            // Create hidden host object
            hostGO = new GameObject("QualityOfLifeHotkeysHost");
            Object.DontDestroyOnLoad(hostGO);

            behaviour = hostGO.AddComponent<QualityOfLifeHotkeysBehaviour>();
            behaviour.Setup(this);
        }

        public override void Unload()
        {
            if (hostGO != null) Object.Destroy(hostGO);
        }

        // Expose keys to behaviour
        public KeyCode DashKey => KeyToggleDash.Value;
        public KeyCode WallJumpKey => KeyToggleWallJump.Value;
        public KeyCode DoubleJumpKey => KeyToggleDoubleJump.Value;
        public KeyCode SuperDashKey => KeyToggleSuperDash.Value;
        public KeyCode ShadowDashKey => KeyToggleShadowDash.Value;
        public KeyCode CycloneKey => KeyToggleCyclone.Value;
        public KeyCode DashSlashKey => KeyToggleDashSlash.Value;
        public KeyCode DreamNailKey => KeyToggleDreamNail.Value;
        public KeyCode DreamGateKey => KeyToggleDreamGate.Value;

        public KeyCode HealthKey => KeyCycleHealth.Value;
        public KeyCode NailKey => KeySetNailDamage.Value;
        public KeyCode FireballKey => KeyCycleFireball.Value;
        public KeyCode QuakeKey => KeyCycleQuake.Value;
        public KeyCode ScreamKey => KeyCycleScream.Value;
    }

    public class QualityOfLifeHotkeysBehaviour : MonoBehaviour
    {
        private QualityOfLifeHotkeysModule module;
        private Knight.PlayerData PD => Knight.PlayerData.instance;

        public void Setup(QualityOfLifeHotkeysModule mod) => module = mod;

        private void Update()
        {
            if (PD == null) return;

            // Toggles
            if (Input.GetKeyDown(module.DashKey)) { PD.canDash = !PD.canDash; PD.hasDash = PD.canDash; }
            if (Input.GetKeyDown(module.WallJumpKey)) { PD.canWallJump = !PD.canWallJump; PD.hasWalljump = PD.canWallJump; }
            if (Input.GetKeyDown(module.SuperDashKey)) { PD.canSuperDash = !PD.canSuperDash; PD.hasSuperDash = PD.canSuperDash; }
            if (Input.GetKeyDown(module.ShadowDashKey)) { PD.canShadowDash = !PD.canShadowDash; PD.hasShadowDash = PD.canShadowDash; }
            if (Input.GetKeyDown(module.DoubleJumpKey)) { PD.hasDoubleJump = !PD.hasDoubleJump; }

            if (Input.GetKeyDown(module.CycloneKey)) PD.hasCyclone = !PD.hasCyclone;
            if (Input.GetKeyDown(module.DashSlashKey)) PD.hasDashSlash = !PD.hasDashSlash;
            if (Input.GetKeyDown(module.DreamNailKey)) PD.hasDreamNail = !PD.hasDreamNail;
            if (Input.GetKeyDown(module.DreamGateKey)) PD.hasDreamGate = !PD.hasDreamGate;

            // Health cycle
            if (Input.GetKeyDown(module.HealthKey))
            {
                PD.maxHealth = (PD.maxHealth < 5 || PD.maxHealth >= 9) ? 5 : PD.maxHealth + 1;
                PD.health = PD.maxHealth;

                ("Max health set to " + PD.maxHealth + " (current " + PD.health + ")").LogInfo();

                // small hack for refreshing HUD but might not be working
                HeroController.instance.AddHealth(0);
            }

            // Nail damage cycle (H)
            if (Input.GetKeyDown(module.NailKey))
            {
                int[] cycle = { 5, 9, 13, 17, 21 };
                int idx = System.Array.IndexOf(cycle, PD.nailDamage);
                PD.nailDamage = (idx == -1 || idx == cycle.Length - 1) ? cycle[0] : cycle[idx + 1];

                ("Nail damage set to " + PD.nailDamage).LogInfo();

                // small hack for refreshing HUD but might not be working
                HeroController.instance.AddHealth(0);
            }

            // Fireball cycle (J)
            if (Input.GetKeyDown(module.FireballKey)) PD.fireballLevel = (PD.fireballLevel + 1) % 3;

            // Quake cycle (K)
            if (Input.GetKeyDown(module.QuakeKey)) PD.quakeLevel = (PD.quakeLevel + 1) % 3;

            // Scream cycle (L)
            if (Input.GetKeyDown(module.ScreamKey)) PD.screamLevel = (PD.screamLevel + 1) % 3;
        }
    }
}

