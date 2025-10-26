using HutongGames.PlayMaker;
using KIS;
using KIS.Utils;

[HarmonyPatch(typeof(ObjectPool), "CreatePool", [typeof(GameObject), typeof(int), typeof(bool), typeof(Vector3), typeof(Quaternion), typeof(bool)])]
public class Patch_ObjectPool_CreatePool : GeneralPatch
{
    public static bool Prefix(GameObject prefab, int initialPoolSize, ref bool setPosition, Vector3 position, Quaternion rotation, bool runInitialisation = false)
    {
        if (KnightInSilksong.IsKnight)
        {
            (prefab.name + " " + setPosition + " ").LogInfo();
            if (prefab.name.Contains("Fireball"))
            {
                var fsm = prefab.GetComponent<PlayMakerFSM>();
                if (fsm != null && fsm.GetVariable<FsmBool>("FromKnight") != null)
                {
                    setPosition = false;
                }
            }
            if (prefab.name == "Audio Player Actor" || prefab.name == "Audio Player Actor 2D")
            {
                setPosition = false;
            }
        }
        return true;
    }
    public static void Postfix()
    {
    }
}