using BepInEx.Bootstrap;
using Newtonsoft.Json.UnityConverters.Helpers;

namespace KIS.Compatibility;

[AttributeUsage(AttributeTargets.Class)]
public class RequiresModAttribute : Attribute
{
    public string Id { get; }

    public RequiresModAttribute(string mod_id)
    {
        this.Id = mod_id;
    }

}

public interface ICompatibility
{
    string ModId { get; }
    string ModName { get; }
    public void Init();
    public void Update();

}
public static class CompatibilityManager
{
    private static List<ICompatibility> compatibilities = new List<ICompatibility>();

    public static void CheckAndInit()
    {
        "Checking for compatible mods...".LogDebug();
        foreach (var type in Assembly.GetExecutingAssembly().GetTypesSafely())
        {
            type.Name.LogDebug();
            var requiresModAttribute = type.GetCustomAttribute<RequiresModAttribute>();
            if (requiresModAttribute != null)
            {
                if (Chainloader.PluginInfos.ContainsKey(requiresModAttribute.Id))
                {
                    if (Activator.CreateInstance(type) is ICompatibility compatibility)
                    {
                        $"Initializing compatibility for mod: {compatibility.ModName}".LogInfo();
                        compatibility.Init();
                        compatibilities.Add(compatibility);
                    }
                }
                else
                {
                    Debug.Log($"Mod with ID {requiresModAttribute.Id} not found. Skipping compatibility for {type.Name}.");
                }
            }
        }
    }
    public static void UpdateAll()
    {
        foreach (var compatibility in compatibilities)
        {
            compatibility.Update();
        }
    }

}