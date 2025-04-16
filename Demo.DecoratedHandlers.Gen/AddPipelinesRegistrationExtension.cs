using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.DecoratedHandlers.Gen;

public static class AddPipelinesRegistrationExtension
{
    private static readonly Type MarkerAttributeType = typeof(RegisterThis);

    /// <summary>
    ///     This method has to be called after all handlers are registered.
    ///     It replaces "raw" handler registrations with source-generated pipelines.
    /// </summary>
    public static void AddPipelines(this IServiceCollection services)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        object[] parameters = [services];

        foreach (var assembly in assemblies)
        {
            foreach (var type in assembly.GetTypes())
            {
                var methods = type.GetMethods(BindingFlags.Static | BindingFlags.NonPublic)
                    .Where(m => m.GetCustomAttributes(MarkerAttributeType, false).Length > 0);

                foreach (var method in methods)
                {
                    method.Invoke(null, parameters);
                }
            }
        }
    }

    public static List<string> Debug()
    {
        var list = new List<string>();
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (var assembly in assemblies)
        {
            if (assembly.FullName.Contains("Demo"))
                list.Add(assembly.FullName);

            foreach (var type in assembly.GetTypes())
            {
                if (type.Name.Contains("Handler"))
                {
                    list.Add(type.FullName);
                }
            }
        }
        return list;
    }
}