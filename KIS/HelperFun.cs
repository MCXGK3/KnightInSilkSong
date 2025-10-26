using System.IO;
using System.Linq;
using KIS;
using UnityEngine;

public static class HelperFun
{
    public static string GetPlayerDataPath()
    {
        return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "PlayerData.json");
    }
    public static void SavePlayerData()
    {
        Knight.PlayerData pd = Knight.PlayerData.instance;
        if (pd == null) return;
        string json = JsonUtility.ToJson(pd, true);
        File.WriteAllText(GetPlayerDataPath(), json);
    }
    public static bool LoadPlayerData()
    {
        string path = GetPlayerDataPath();
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            try
            {
                Knight.PlayerData pd = Knight.PlayerData.instance;
                JsonUtility.FromJsonOverwrite(json, pd);
                "Load OK".LogInfo();
            }
            catch (Exception e)
            {
                e.LogError();
                return false;
            }
            return true;
        }
        return false;
    }
    public static Texture2D LoadTexture(Stream stream)
    {
        byte[] bytes = new byte[stream.Length];
        stream.Read(bytes, 0, bytes.Length);
        stream.Close();

        // 创建Texture2D并加载图片数据
        Texture2D texture = new Texture2D(2, 2);
        if (texture.LoadImage(bytes))
        {
            return texture;
        }
        return null;
    }
    public static Texture2D LoadTexture(string path)
    {
        // 创建文件流
        FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
        byte[] bytes = new byte[fileStream.Length];
        fileStream.Read(bytes, 0, bytes.Length);
        fileStream.Close();

        // 创建Texture2D并加载图片数据
        Texture2D texture = new Texture2D(2, 2);
        if (texture.LoadImage(bytes))
        {
            return texture;
        }
        return null;
    }
    public static Component GetComponent(GameObject gameObject, string type)
    {
        return gameObject.GetComponent(type);
    }
    public static Component GetAnyComponent<T1, T2>(this GameObject gameObject) where T1 : Component where T2 : Component
    {
        var c1 = gameObject.GetComponent<T1>();
        if (c1 != null) return c1;
        var c2 = gameObject.GetComponent<T2>();
        if (c2 != null) return c2;
        return null;
    }
    public static Component GetAnotherComponent(this Component monoBehaviour)
    {
        bool is_knight = false;
        var fullName = monoBehaviour.GetType().FullName;
        if (fullName.StartsWith("Knight"))
        {
            is_knight = true;
        }
        var name = fullName.Split(".").Last();
        if (!is_knight) return Knight.HeroController.instance.GetComponent(name);
        else return HeroController.instance.GetComponent(name);
    }
    public static void LogInfo(this object msg)
    {
        KnightInSilksong.logger.LogInfo(msg);
    }
    public static void LogWarning(this object msg)
    {
        KnightInSilksong.logger.LogWarning(msg);
    }
    public static void LogError(this object msg)
    {
        KnightInSilksong.logger.LogError(msg);
    }
    public static void LogDebug(this object msg)
    {
        KnightInSilksong.logger.LogDebug(msg);
    }
    public static void LogFatal(this object msg)
    {
        KnightInSilksong.logger.LogFatal(msg);
    }

}