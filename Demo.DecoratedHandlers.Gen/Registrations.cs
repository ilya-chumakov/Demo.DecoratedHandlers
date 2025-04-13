using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.DecoratedHandlers.Gen;

public static class Registrations
{
    public static void ApplyGeneratedRegistrations(this IServiceCollection services)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        foreach (var assembly in assemblies)
        {
            foreach (var type in assembly.GetTypes())
            {
                var methods = type.GetMethods(BindingFlags.Static | BindingFlags.Public)
                    .Where(m => m.GetCustomAttributes(typeof(RegisterThis), false).Any());

                foreach (var method in methods)
                {
                    method.Invoke(null, [services]);
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