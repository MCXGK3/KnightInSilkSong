using System.Collections;
using Mono.Posix;
using ToJ;
using UnityEngine.UI;
using static ToJ.Mask;

// [HarmonyPatch(typeof(ToJ.Mask), "Update", MethodType.Normal)]
public class Patch_Mask_Update
{
    public static IDictionary valuePairs;
    public static List<Material> check_list = null;
    public static bool Prefix(ToJ.Mask __instance)
    {
        OrigUpdate(__instance);
        return false;
    }
    public static void Postfix(ToJ.Mask __instance)
    {
    }
    private static void ModifiedUpdate(ToJ.Mask __instance)
    {
        if (__instance._maskedSpriteWorldCoordsShader == null)
        {
            __instance._maskedSpriteWorldCoordsShader = Shader.Find("Alpha Masked/Sprites Alpha Masked - World Coords");
        }

        if (__instance._maskedUnlitWorldCoordsShader == null)
        {
            __instance._maskedUnlitWorldCoordsShader = Shader.Find("Alpha Masked/Unlit Alpha Masked - World Coords");
        }

        if (__instance._maskedSpriteWorldCoordsShader == null || __instance._maskedUnlitWorldCoordsShader == null)
        {
            if (!__instance.shaderErrorLogged)
            {
                Debug.LogError("Shaders necessary for masking don't seem to be present in the project.");
            }
        }
        else
        {
            if (!__instance.transform.hasChanged)
            {
                return;
            }
            "Begin Change".LogInfo();
            __instance.transform.hasChanged = false;
            if (__instance.maskMappingWorldAxis == MappingAxis.X && (Mathf.Abs(Mathf.DeltaAngle(__instance.transform.eulerAngles.x, 0f)) > 0.01f || Mathf.Abs(Mathf.DeltaAngle(__instance.transform.eulerAngles.y, __instance.invertAxis ? (-90) : 90)) > 0.01f))
            {
                __instance.transform.eulerAngles = new Vector3(0f, __instance.invertAxis ? 270 : 90, __instance.transform.eulerAngles.z);
            }
            else if (__instance.maskMappingWorldAxis == MappingAxis.Y && (Mathf.Abs(Mathf.DeltaAngle(__instance.transform.eulerAngles.x, __instance.invertAxis ? (-90) : 90)) > 0.01f || Mathf.Abs(Mathf.DeltaAngle(__instance.transform.eulerAngles.z, 0f)) > 0.01f))
            {
                __instance.transform.eulerAngles = new Vector3(__instance.invertAxis ? (-90) : 90, __instance.transform.eulerAngles.y, 0f);
            }
            else if (__instance.maskMappingWorldAxis == MappingAxis.Z && (Mathf.Abs(Mathf.DeltaAngle(__instance.transform.eulerAngles.x, 0f)) > 0.01f || Mathf.Abs(Mathf.DeltaAngle(__instance.transform.eulerAngles.y, __instance.invertAxis ? (-180) : 0)) > 0.01f))
            {
                __instance.transform.eulerAngles = new Vector3(0f, __instance.invertAxis ? (-180) : 0, __instance.transform.eulerAngles.z);
            }

            if (!(__instance.transform.parent != null))
            {
                return;
            }
            "Parent Changed".LogInfo();

            Renderer[] componentsInChildren = __instance.transform.parent.gameObject.GetComponentsInChildren<Renderer>();
            Graphic[] componentsInChildren2 = __instance.transform.parent.gameObject.GetComponentsInChildren<Graphic>();
            List<Material> list = new List<Material>();
            check_list = list;
            Dictionary<Material, Graphic> dictionary = new Dictionary<Material, Graphic>();
            valuePairs = dictionary;/////////////////////////////////////////
            Renderer[] array = componentsInChildren;
            foreach (Renderer renderer in array)
            {
                if (!(renderer.gameObject != __instance.gameObject))
                {
                    continue;
                }

                Material[] sharedMaterials = renderer.sharedMaterials;
                foreach (Material item in sharedMaterials)
                {
                    if (!list.Contains(item))
                    {
                        list.Add(item);
                    }
                }
            }

            Graphic[] array2 = componentsInChildren2;
            foreach (Graphic graphic in array2)
            {
                if (graphic.gameObject != __instance.gameObject && !list.Contains(graphic.material))
                {
                    list.Add(graphic.material);
                    Canvas canvas = graphic.canvas;
                    if (canvas.renderMode == RenderMode.ScreenSpaceOverlay || (canvas.renderMode == RenderMode.ScreenSpaceCamera && canvas.worldCamera == null))
                    {
                        dictionary.Add(list[list.Count - 1], graphic);
                    }
                }
            }

            foreach (Material item2 in list)
            {
                if (item2.shader.ToString() == __instance._maskedSpriteWorldCoordsShader.ToString() && item2.shader.GetInstanceID() != __instance._maskedSpriteWorldCoordsShader.GetInstanceID())
                {
                    __instance._maskedSpriteWorldCoordsShader = null;
                }

                if (item2.shader.ToString() == __instance._maskedUnlitWorldCoordsShader.ToString() && item2.shader.GetInstanceID() != __instance._maskedUnlitWorldCoordsShader.GetInstanceID())
                {
                    __instance._maskedUnlitWorldCoordsShader = null;
                }

                if (!(item2.shader == __instance._maskedSpriteWorldCoordsShader) && !(item2.shader == __instance._maskedUnlitWorldCoordsShader))
                {
                    continue;
                }

                item2.DisableKeyword("_SCREEN_SPACE_UI");
                Vector2 value = new Vector2(1f / __instance.transform.lossyScale.x, 1f / __instance.transform.lossyScale.y);
                Vector2 vector = Vector2.zero;
                float num = 0f;
                int num2 = 1;
                if (__instance.maskMappingWorldAxis == MappingAxis.X)
                {
                    num2 = (__instance.invertAxis ? 1 : (-1));
                    vector = new Vector2(0f - __instance.transform.position.z, 0f - __instance.transform.position.y);
                    num = (float)num2 * __instance.transform.eulerAngles.z;
                }
                else if (__instance.maskMappingWorldAxis == MappingAxis.Y)
                {
                    vector = new Vector2(0f - __instance.transform.position.x, 0f - __instance.transform.position.z);
                    num = 0f - __instance.transform.eulerAngles.y;
                }
                else if (__instance.maskMappingWorldAxis == MappingAxis.Z)
                {
                    num2 = ((!__instance.invertAxis) ? 1 : (-1));
                    vector = new Vector2(0f - __instance.transform.position.x, 0f - __instance.transform.position.y);
                    num = (float)num2 * __instance.transform.eulerAngles.z;
                }

                RectTransform component = __instance.GetComponent<RectTransform>();
                if (component != null)
                {
                    Rect rect = component.rect;
                    vector += (Vector2)(__instance.transform.right * (component.pivot.x - 0.5f) * rect.width * __instance.transform.lossyScale.x + __instance.transform.up * (component.pivot.y - 0.5f) * rect.height * __instance.transform.lossyScale.y);
                    value.x /= rect.width;
                    value.y /= rect.height;
                }

                if (dictionary.ContainsKey(item2))
                {
                    vector = dictionary[item2].transform.InverseTransformVector(vector);
                    switch (__instance.maskMappingWorldAxis)
                    {
                        case MappingAxis.X:
                            vector.x *= dictionary[item2].transform.lossyScale.z;
                            vector.y *= dictionary[item2].transform.lossyScale.y;
                            break;
                        case MappingAxis.Y:
                            vector.x *= dictionary[item2].transform.lossyScale.x;
                            vector.y *= dictionary[item2].transform.lossyScale.z;
                            break;
                        case MappingAxis.Z:
                            vector.x *= dictionary[item2].transform.lossyScale.x;
                            vector.y *= dictionary[item2].transform.lossyScale.y;
                            break;
                    }

                    Canvas canvas2 = dictionary[item2].canvas;
                    vector /= canvas2.scaleFactor;
                    vector = __instance.RotateVector(vector, dictionary[item2].transform.eulerAngles);
                    vector += canvas2.GetComponent<RectTransform>().sizeDelta * 0.5f;
                    value *= canvas2.scaleFactor;
                    item2.EnableKeyword("_SCREEN_SPACE_UI");
                }

                Vector2 mainTextureScale = __instance.gameObject.GetComponent<Renderer>().sharedMaterial.mainTextureScale;
                value.x *= mainTextureScale.x;
                value.y *= mainTextureScale.y;
                value.x *= num2;
                Vector2 vector2 = vector;
                float num3 = Mathf.Sin((0f - num) * (MathF.PI / 180f));
                float num4 = Mathf.Cos((0f - num) * (MathF.PI / 180f));
                vector.x = (num4 * vector2.x - num3 * vector2.y) * value.x + 0.5f * mainTextureScale.x;
                vector.y = (num3 * vector2.x + num4 * vector2.y) * value.y + 0.5f * mainTextureScale.y;
                vector += __instance.gameObject.GetComponent<Renderer>().sharedMaterial.mainTextureOffset;
                ("Vector: " + vector.ToString()).LogInfo();
                ("Value: " + value.ToString()).LogInfo();
                ("Num: " + num.ToString()).LogInfo();
                ("Num in Radians: " + (num * (MathF.PI / 180f)).ToString()).LogInfo();
                item2.SetTextureOffset("_AlphaTex", vector);
                item2.SetTextureScale("_AlphaTex", value);
                item2.SetFloat("_MaskRotation", num * (MathF.PI / 180f));
            }

        }
    }

    private static void OrigUpdate(ToJ.Mask __instance)
    {
        if (__instance._maskedSpriteWorldCoordsShader == null)
        {
            __instance._maskedSpriteWorldCoordsShader = Shader.Find("Alpha Masked/Sprites Alpha Masked - World Coords");
        }

        if (__instance._maskedUnlitWorldCoordsShader == null)
        {
            __instance._maskedUnlitWorldCoordsShader = Shader.Find("Alpha Masked/Unlit Alpha Masked - World Coords");
        }

        if (__instance._maskedSpriteWorldCoordsShader == null || __instance._maskedUnlitWorldCoordsShader == null)
        {
            if (!__instance.shaderErrorLogged)
            {
                Debug.LogError("Shaders necessary for masking don't seem to be present in the project.");
            }
        }
        else
        {
            if (!__instance.transform.hasChanged)
            {
                return;
            }

            __instance.transform.hasChanged = false;
            if (__instance.maskMappingWorldAxis == MappingAxis.X && (Mathf.Abs(Mathf.DeltaAngle(__instance.transform.eulerAngles.x, 0f)) > 0.01f || Mathf.Abs(Mathf.DeltaAngle(__instance.transform.eulerAngles.y, __instance.invertAxis ? (-90) : 90)) > 0.01f))
            {
                __instance.transform.eulerAngles = new Vector3(0f, __instance.invertAxis ? 270 : 90, __instance.transform.eulerAngles.z);
            }
            else if (__instance.maskMappingWorldAxis == MappingAxis.Y && (Mathf.Abs(Mathf.DeltaAngle(__instance.transform.eulerAngles.x, __instance.invertAxis ? (-90) : 90)) > 0.01f || Mathf.Abs(Mathf.DeltaAngle(__instance.transform.eulerAngles.z, 0f)) > 0.01f))
            {
                __instance.transform.eulerAngles = new Vector3(__instance.invertAxis ? (-90) : 90, __instance.transform.eulerAngles.y, 0f);
            }
            else if (__instance.maskMappingWorldAxis == MappingAxis.Z && (Mathf.Abs(Mathf.DeltaAngle(__instance.transform.eulerAngles.x, 0f)) > 0.01f || Mathf.Abs(Mathf.DeltaAngle(__instance.transform.eulerAngles.y, __instance.invertAxis ? (-180) : 0)) > 0.01f))
            {
                __instance.transform.eulerAngles = new Vector3(0f, __instance.invertAxis ? (-180) : 0, __instance.transform.eulerAngles.z);
            }

            if (!(__instance.transform.parent != null))
            {
                return;
            }

            Renderer[] componentsInChildren = __instance.transform.parent.gameObject.GetComponentsInChildren<Renderer>();
            Graphic[] componentsInChildren2 = __instance.transform.parent.gameObject.GetComponentsInChildren<Graphic>();
            List<Material> list = new List<Material>();
            Dictionary<Material, Graphic> dictionary = new Dictionary<Material, Graphic>();
            Renderer[] array = componentsInChildren;
            foreach (Renderer renderer in array)
            {
                if (!(renderer.gameObject != __instance.gameObject))
                {
                    continue;
                }

                Material[] sharedMaterials = renderer.sharedMaterials;
                foreach (Material item in sharedMaterials)
                {
                    if (!list.Contains(item))
                    {
                        list.Add(item);
                    }
                }
            }

            Graphic[] array2 = componentsInChildren2;
            foreach (Graphic graphic in array2)
            {
                if (graphic.gameObject != __instance.gameObject && !list.Contains(graphic.material))
                {
                    list.Add(graphic.material);
                    Canvas canvas = graphic.canvas;
                    if (canvas.renderMode == RenderMode.ScreenSpaceOverlay || (canvas.renderMode == RenderMode.ScreenSpaceCamera && canvas.worldCamera == null))
                    {
                        dictionary.Add(list[list.Count - 1], graphic);
                    }
                }
            }

            foreach (Material item2 in list)
            {
                if (item2.shader.ToString() == __instance._maskedSpriteWorldCoordsShader.ToString() && item2.shader.GetInstanceID() != __instance._maskedSpriteWorldCoordsShader.GetInstanceID())
                {
                    __instance._maskedSpriteWorldCoordsShader = null;
                }

                if (item2.shader.ToString() == __instance._maskedUnlitWorldCoordsShader.ToString() && item2.shader.GetInstanceID() != __instance._maskedUnlitWorldCoordsShader.GetInstanceID())
                {
                    __instance._maskedUnlitWorldCoordsShader = null;
                }

                if (!(item2.shader == __instance._maskedSpriteWorldCoordsShader) && !(item2.shader == __instance._maskedUnlitWorldCoordsShader))
                {
                    continue;
                }

                item2.DisableKeyword("_SCREEN_SPACE_UI");
                Vector2 value = new Vector2(1f / __instance.transform.lossyScale.x, 1f / __instance.transform.lossyScale.y);
                Vector2 vector = Vector2.zero;
                float num = 0f;
                int num2 = 1;
                if (__instance.maskMappingWorldAxis == MappingAxis.X)
                {
                    num2 = (__instance.invertAxis ? 1 : (-1));
                    vector = new Vector2(0f - __instance.transform.position.z, 0f - __instance.transform.position.y);
                    num = (float)num2 * __instance.transform.eulerAngles.z;
                }
                else if (__instance.maskMappingWorldAxis == MappingAxis.Y)
                {
                    vector = new Vector2(0f - __instance.transform.position.x, 0f - __instance.transform.position.z);
                    num = 0f - __instance.transform.eulerAngles.y;
                }
                else if (__instance.maskMappingWorldAxis == MappingAxis.Z)
                {
                    num2 = ((!__instance.invertAxis) ? 1 : (-1));
                    vector = new Vector2(0f - __instance.transform.position.x, 0f - __instance.transform.position.y);
                    num = (float)num2 * __instance.transform.eulerAngles.z;
                }

                RectTransform component = __instance.GetComponent<RectTransform>();
                if (component != null)
                {
                    Rect rect = component.rect;
                    vector += (Vector2)(__instance.transform.right * (component.pivot.x - 0.5f) * rect.width * __instance.transform.lossyScale.x + __instance.transform.up * (component.pivot.y - 0.5f) * rect.height * __instance.transform.lossyScale.y);
                    value.x /= rect.width;
                    value.y /= rect.height;
                }

                if (dictionary.ContainsKey(item2))
                {
                    vector = dictionary[item2].transform.InverseTransformVector(vector);
                    switch (__instance.maskMappingWorldAxis)
                    {
                        case MappingAxis.X:
                            vector.x *= dictionary[item2].transform.lossyScale.z;
                            vector.y *= dictionary[item2].transform.lossyScale.y;
                            break;
                        case MappingAxis.Y:
                            vector.x *= dictionary[item2].transform.lossyScale.x;
                            vector.y *= dictionary[item2].transform.lossyScale.z;
                            break;
                        case MappingAxis.Z:
                            vector.x *= dictionary[item2].transform.lossyScale.x;
                            vector.y *= dictionary[item2].transform.lossyScale.y;
                            break;
                    }

                    Canvas canvas2 = dictionary[item2].canvas;
                    vector /= canvas2.scaleFactor;
                    vector = __instance.RotateVector(vector, dictionary[item2].transform.eulerAngles);
                    vector += canvas2.GetComponent<RectTransform>().sizeDelta * 0.5f;
                    value *= canvas2.scaleFactor;
                    item2.EnableKeyword("_SCREEN_SPACE_UI");
                }

                Vector2 mainTextureScale = __instance.gameObject.GetComponent<Renderer>().sharedMaterial.mainTextureScale;
                value.x *= mainTextureScale.x;
                value.y *= mainTextureScale.y;
                value.x *= num2;
                Vector2 vector2 = vector;
                float num3 = Mathf.Sin((0f - num) * (MathF.PI / 180f));
                float num4 = Mathf.Cos((0f - num) * (MathF.PI / 180f));
                vector.x = (num4 * vector2.x - num3 * vector2.y) * value.x + 0.5f * mainTextureScale.x;
                vector.y = (num3 * vector2.x + num4 * vector2.y) * value.y + 0.5f * mainTextureScale.y;
                vector += __instance.gameObject.GetComponent<Renderer>().sharedMaterial.mainTextureOffset;
                item2.SetTextureOffset("_AlphaTex", vector);
                item2.SetTextureScale("_AlphaTex", value);
                item2.SetFloat("_MaskRotation", num * (MathF.PI / 180f));
            }

        }
    }

}