

using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using KIS;
using UnityEngine;
using UnityEngine.SceneManagement;


//
// 摘要:
//     Utilities to work with GameObjects
public static class GameObjectUtils
{
    //
    // 摘要:
    //     Get a component of type T if it exists on the GameObject or add a new one
    //
    // 参数:
    //   go:
    //     GameObject
    //
    // 类型参数:
    //   T:
    //     Component Type
    //
    // 返回结果:
    //     A component of type T
    public static T GetAddComponent<T>(this GameObject go) where T : Component
    {
        T val = go.GetComponent<T>();
        if (val == null)
        {
            val = go.AddComponent<T>();
        }

        return val;
    }

    //
    // 摘要:
    //     Remove a Component from the GameObject
    //
    // 参数:
    //   go:
    //     GameObject
    //
    // 类型参数:
    //   T:
    //     Component Type
    //
    // 返回结果:
    //     boolean indicating if the component was removed
    public static bool RemoveComponent<T>(this GameObject go) where T : Component
    {
        T component = go.GetComponent<T>();
        if (component != null)
        {
            Object.DestroyImmediate(component);
            return true;
        }

        return false;
    }

    //
    // 摘要:
    //     Copy a component from one GameObject to another
    //
    // 参数:
    //   to:
    //     GameObject to copy to
    //
    //   from:
    //     GameObject to copy from
    //
    // 类型参数:
    //   T:
    //     Component Type
    //
    // 返回结果:
    //     the copied component
    public static T copyComponent<T>(this GameObject to, GameObject from) where T : Component
    {
        if (from == null)
        {
            return null;
        }

        T component = from.GetComponent<T>();
        T addComponent = to.GetAddComponent<T>();
        if (component == null)
        {
            return null;
        }

        FieldInfo[] fields = typeof(T).GetFields(BindingFlags.IgnoreCase | BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.InvokeMethod | BindingFlags.CreateInstance | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty | BindingFlags.PutDispProperty | BindingFlags.PutRefDispProperty | BindingFlags.ExactBinding | BindingFlags.SuppressChangeType | BindingFlags.OptionalParamBinding | BindingFlags.IgnoreReturn);
        foreach (FieldInfo fieldInfo in fields)
        {
            fieldInfo.SetValue(addComponent, fieldInfo.GetValue(component));
        }

        return addComponent;
    }

    //
    // 摘要:
    //     Set the scale of a GameObject
    //
    // 参数:
    //   gameObject:
    //     GameObject
    //
    //   scaleX:
    //     X scale
    //
    //   scaleY:
    //     Y scale
    public static void SetScale(this GameObject gameObject, float scaleX, float scaleY)
    {
        Vector3 localScale = gameObject.transform.localScale;
        localScale.x = scaleX;
        localScale.y = scaleY;
        gameObject.transform.localScale = localScale;
    }

    //
    // 摘要:
    //     Find a gameobject in children of another by name
    //
    // 参数:
    //   gameObject:
    //     GameObject parent
    //
    //   name:
    //     Name of GameObject to find
    //
    //   useBaseName:
    //     boolean indicaing if baseName should be used
    //
    // 返回结果:
    //     The GameObject if found or null
    public static GameObject FindGameObjectInChildren(this GameObject gameObject, string name, bool useBaseName = false)
    {
        if (gameObject == null)
        {
            return null;
        }

        Transform[] componentsInChildren = gameObject.GetComponentsInChildren<Transform>(includeInactive: true);
        foreach (Transform transform in componentsInChildren)
        {
            if (transform.GetName(useBaseName) == name)
            {
                return transform.gameObject;
            }
        }

        return null;
    }

    //
    // 摘要:
    //     Find all gameobjects in children of another by name
    //
    // 参数:
    //   gameObject:
    //     GameObject parent
    //
    //   name:
    //     Name of GameObjects to find
    //
    //   useBaseName:
    //     boolean indicaing if baseName should be used
    //
    // 返回结果:
    //     The List of GameObjects
    public static List<GameObject> FindGameObjectsInChildren(this GameObject gameObject, string name, bool useBaseName = false)
    {
        if (gameObject == null)
        {
            return null;
        }

        List<GameObject> list = new List<GameObject>();
        Transform[] componentsInChildren = gameObject.GetComponentsInChildren<Transform>(includeInactive: true);
        foreach (Transform transform in componentsInChildren)
        {
            if (transform.GetName(useBaseName) == name)
            {
                list.Add(transform.gameObject);
            }
        }

        return list;
    }

    //
    // 摘要:
    //     Log a game object for debugging
    //
    // 参数:
    //   gameObject:
    //     GameObject
    public static void Log(this GameObject gameObject)
    {
        if (gameObject == null)
        {
            return;
        }

        KnightInSilksong.logger.LogInfo(gameObject.GetName());
        KnightInSilksong.logger.LogInfo(gameObject.GetPath());
        KnightInSilksong.logger.LogInfo("Layer : " + gameObject.layer);
        KnightInSilksong.logger.LogInfo("Position : " + gameObject.transform.position.ToString());
        KnightInSilksong.logger.LogInfo("Rotation : " + gameObject.transform.rotation.ToString());
        KnightInSilksong.logger.LogInfo("Scale : " + gameObject.transform.localScale.ToString());
        Component[] components = gameObject.GetComponents<Component>();
        foreach (Component component in components)
        {
            KnightInSilksong.logger.LogInfo("Component : " + component.GetType());
            if (component is PlayMakerFSM)
            {
                KnightInSilksong.logger.LogInfo("---- Fsm name :" + (component as PlayMakerFSM).FsmName);
            }
        }
    }

    //
    // 摘要:
    //     Log a game object and all it's children for debugging
    //
    // 参数:
    //   gameObject:
    public static void LogWithChildren(this GameObject gameObject)
    {
        if (!(gameObject == null))
        {
            gameObject.Log();
            Transform[] componentsInChildren = gameObject.GetComponentsInChildren<Transform>(includeInactive: true);
            for (int i = 0; i < componentsInChildren.Length; i++)
            {
                componentsInChildren[i].gameObject.Log();
            }
        }
    }

    //
    // 摘要:
    //     Logs all active gameObjects
    public static void PrintAllActiveGameObjectsInScene()
    {
        GameObject[] array = Object.FindObjectsByType<GameObject>(sortMode: FindObjectsSortMode.None);
        for (int i = 0; i < array.Length; i++)
        {
            array[i].Log();
        }
    }

    //
    // 摘要:
    //     Find a GameObject that is a descendent of the current GameObject by name
    //
    // 参数:
    //   go:
    //     Ancestor GameObject
    //
    //   name:
    //     Name of GameObject to find
    //
    // 返回结果:
    //     GameObject or null
    public static GameObject Find(this GameObject go, string name)
    {
        for (int i = 0; i < go.transform.childCount; i++)
        {
            GameObject gameObject = go.transform.GetChild(i).gameObject;
            if (gameObject.name == name)
            {
                return gameObject;
            }
        }

        for (int j = 0; j < go.transform.childCount; j++)
        {
            GameObject gameObject2 = Find(go.transform.GetChild(j).gameObject, name);
            if (gameObject2 != null)
            {
                return gameObject2;
            }
        }

        return null;
    }

    //
    // 摘要:
    //     Find all GameObjects that are a descendent of the current GameObject by name
    //
    //
    // 参数:
    //   go:
    //     Ancestor GameObject
    //
    //   allGoList:
    //     List to add the results to
    public static void FindAllChildren(this GameObject go, List<GameObject> allGoList)
    {
        for (int i = 0; i < go.transform.childCount; i++)
        {
            allGoList.Add(go.transform.GetChild(i).gameObject);
        }

        for (int j = 0; j < go.transform.childCount; j++)
        {
            go.transform.GetChild(j).gameObject.FindAllChildren(allGoList);
        }
    }

    //
    // 摘要:
    //     Disable all children of a GameObject
    //
    // 参数:
    //   go:
    //     GameObject
    public static void DisableChildren(this GameObject go)
    {
        for (int i = 0; i < go.transform.childCount; i++)
        {
            go.transform.GetChild(i).gameObject.SetActive(value: false);
        }
    }

    //
    // 摘要:
    //     Get All gameobjects in scene (even inactive ones)
    //
    // 参数:
    //   scene:
    //     The scene
    //
    // 返回结果:
    //     A List of All GameObjects in the scene
    public static List<GameObject> GetAllGameObjects(this Scene scene)
    {
        List<GameObject> list = new List<GameObject>();
        GameObject[] rootGameObjects = scene.GetRootGameObjects();
        foreach (GameObject gameObject in rootGameObjects)
        {
            list.Add(gameObject);
            gameObject.FindAllChildren(list);
        }

        return list;
    }

    //
    // 摘要:
    //     Get a gameobject by name in a scene (even inactive ones)
    //
    // 参数:
    //   scene:
    //     The scene
    //
    //   name:
    //     Name of the GameObject
    //
    //   useBaseName:
    //     boolean indicating if BaseName should be used
    //
    // 返回结果:
    //     GameObject or null
    public static GameObject GetGameObjectByName(this Scene scene, string name, bool useBaseName = false)
    {
        GameObject[] rootGameObjects = scene.GetRootGameObjects();
        foreach (GameObject gameObject in rootGameObjects)
        {
            if (gameObject.GetName(useBaseName) == name)
            {
                return gameObject;
            }

            GameObject gameObject2 = gameObject.FindGameObjectInChildren(name, useBaseName);
            if (gameObject2 != null)
            {
                return gameObject2;
            }
        }

        return null;
    }

    //
    // 摘要:
    //     Get Root GameObjects in currently Active scene
    public static GameObject[] GetRootGameObjects()
    {
        return UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
    }

    //
    // 摘要:
    //     Find a gameobject by name in the active scene(even inactive ones)
    //
    // 参数:
    //   name:
    //     Name of GameObject
    //
    //   useBaseName:
    //     boolean indicating if BaseName should be used
    //
    // 返回结果:
    //     GameObject or null
    public static GameObject GetGameObjectInScene(string name, bool useBaseName = false)
    {
        return UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetGameObjectByName(name, useBaseName);
    }

    //
    // 摘要:
    //     Get All GameObjects in the active scene (even inactive ones)
    public static List<GameObject> GetAllGameObjectsInScene()
    {
        return UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetAllGameObjects();
    }

    //
    // 摘要:
    //     Get the Name of a GameObject by Transform
    //
    // 参数:
    //   transform:
    //     the Transform
    //
    //   useBaseName:
    //     boolean indicating if BaseName should be used
    public static string GetName(this Transform transform, bool useBaseName = false)
    {
        return transform.gameObject.GetName(useBaseName);
    }

    //
    // 摘要:
    //     Get the Name of a GameObject
    //
    // 参数:
    //   go:
    //     GameObject
    //
    //   useBaseName:
    //     boolean indicating if BaseName should be used
    public static string GetName(this GameObject go, bool useBaseName = false)
    {
        string text = go.name;
        if (useBaseName)
        {
            text = text.ToLower();
            text.Replace("(clone)", "");
            text = text.Trim();
            text.Replace("cln", "");
            text = text.Trim();
            text = Regex.Replace(text, "\\([0-9+]+\\)", "");
            text = text.Trim();
            text = Regex.Replace(text, "[0-9+]+$", "");
            text = text.Trim();
            text.Replace("(clone)", "");
            text = text.Trim();
        }

        return text;
    }

    //
    // 摘要:
    //     Get the Scene path of a GameObject
    //
    // 参数:
    //   go:
    //     GameObject
    //
    //   useBaseName:
    //     boolean indicating if BaseName should be used
    public static string GetPath(this GameObject go, bool useBaseName = false)
    {
        string text = go.GetName(useBaseName);
        GameObject gameObject = go;
        while (gameObject.transform.parent != null && gameObject.transform.parent.gameObject != null)
        {
            gameObject = gameObject.transform.parent.gameObject;
            text = gameObject.GetName(useBaseName) + "/" + text;
        }

        return text;
    }


}
