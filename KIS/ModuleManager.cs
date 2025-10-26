using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mono.Posix;
using KIS;

internal static class ModuleManager
{
    static Dictionary<Type, IModule> modules = new();
    static bool initialized = false;
    public static T GetInstance<T>() where T : IModule
    {
        if (modules.ContainsKey(typeof(T)))
        {
            return (T)modules[typeof(T)];
        }
        return null;
    }

    public static void Init()
    {
        if (!initialized)
        {
            var initializableTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => typeof(IModule).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);
            foreach (var type in initializableTypes)
            {

                try
                {
                    modules.Add(type, (IModule)Activator.CreateInstance(type));
                }
                catch
                {
                    KnightInSilksong.logger.LogError("Fail to Create " + type.FullName);
                }

            }
            initialized = true;
        }

        foreach (var module in modules)
        {
            module.Value.Init();
        }
    }
    public static void Unload()
    {
        foreach (var module in modules)
        {
            module.Value.Unload();
        }
    }
}