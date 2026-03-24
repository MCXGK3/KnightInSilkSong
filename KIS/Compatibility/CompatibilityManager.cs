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
    private static Dictionary<string, ICompatibility> compatibilities = new();


    public static void CheckAndInit()
    {
        "Checking for compatible mods...".LogDebug();
        foreach (var type in Assembly.GetExecutingAssembly().GetTypesSafely())
        {
            var requiresModAttribute = type.GetCustomAttribute<RequiresModAttribute>();
            if (requiresModAttribute != null)
            {
                if (Chainloader.PluginInfos.ContainsKey(requiresModAttribute.Id))
                {
                    if (!compatibilities.ContainsKey(requiresModAttribute.Id))
                    {
                        compatibilities.Add(requiresModAttribute.Id, Activator.CreateInstance(type) as ICompatibility);
                    }
                    compatibilities[requiresModAttribute.Id]?.Init();
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
            compatibility.Value.Update();
        }
    }

}