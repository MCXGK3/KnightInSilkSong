using DebugMod;
namespace KIS.Compatibility
{
    [RequiresMod(DebugMod.DebugMod.Id)]
    public class WithDebugMod : ICompatibility
    {
        public string ModName => "Debug Mod";

        public string ModId => DebugMod.DebugMod.Id;

        public DebugMod.DebugMod debug => DebugMod.DebugMod.instance;
        Knight.PlayerData pd => Knight.PlayerData.instance;
        Knight.HeroController hc => Knight.HeroController.instance;


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
                hc.transform.position = DebugMod.DebugMod.noclipPos;
            }
            if (DebugMod.DebugMod.infiniteSilk)
            {
                if (pd.MPCharge < pd.maxMP)
                {
                    pd.MPCharge = pd.maxMP;
                    hc.SoulGain();
                }
            }

        }
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
    }
}