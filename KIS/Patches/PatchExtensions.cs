using System.Numerics;
using KIS;
using Vector2 = UnityEngine.Vector2;

[HarmonyPatch(typeof(Extensions), "SetPosition2D", [typeof(Transform), typeof(float), typeof(float)])]
public class Patch_Extensions_SetPosition2D : GeneralPatch
{
    public static bool Prefix(Transform t, float x, float y)
    {
        if (KnightInSilksong.IsKnight)
        {
            if (t == HeroController.instance.transform)
            {
                var t2 = Knight.HeroController.instance.transform;
                t2.position = new UnityEngine.Vector3(x, y, t2.position.z);
            }
        }
        return true;
    }
    public static void Postfix(Transform t, float x, float y)
    {
    }
}
[HarmonyPatch(typeof(Extensions), "SetPosition2D", [typeof(Transform), typeof(Vector2)])]
public class Patch_Extensions_SetPosition2D2 : GeneralPatch
{
    public static bool Prefix(Transform t, ref Vector2 position)
    {
        if (KnightInSilksong.IsKnight)
        {
            if (t == HeroController.instance.transform)
            {
                var t2 = Knight.HeroController.instance.transform;
                t2.position = new UnityEngine.Vector3(position.x, position.y, t2.position.z);
            }
        }
        return true;
    }
    public static void Postfix(Transform t, Vector2 position)
    {
    }
}