using HutongGames.PlayMaker;
using KIS;

[HarmonyPatch(typeof(FSMUtility), "SendEventToGameObject", [typeof(GameObject), typeof(FsmEvent), typeof(bool)])]
public class Patch_FSMUtility_SendEventToGameObject : GeneralPatch
{
    public static bool Prefix(GameObject go, FsmEvent ev, bool isRecursive = false)
    {
        if (KnightInSilksong.IsKnight)
        {
            if (HeroController.instance != null)
            {
                if (go == HeroController.instance.gameObject)
                {
                    FSMUtility.SendEventToGameObject(Knight.HeroController.instance.gameObject, ev, isRecursive);
                    return false;
                }
            }
        }
        return true;
    }
    public static void Postfix(GameObject go, FsmEvent ev, bool isRecursive = false)
    {
    }
}