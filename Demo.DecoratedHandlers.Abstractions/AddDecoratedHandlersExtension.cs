using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace Demo.DecoratedHandlers.Abstractions;

// InternalsVisibleTo won't work if directly called from another assembly
public static class AddDecoratedHandlersExtension
{
    private static readonly Type RegistryType = typeof(IPipelineRegistry);

    public static void AddDecoratedHandlers<TPipelineRegistry>(this IServiceCollection services)
        where TPipelineRegistry : IPipelineRegistry, new()
    {
        IPipelineRegistry registry = new TPipelineRegistry();
        registry.Apply(services);
    }

    public static void AddDecoratedHandlers(this IServiceCollection services,
        IEnumerable<Assembly> scanAssemblies = null)
    {
        // todo any better ideas for speeding up this assembly scan?
        var assemblies = scanAssemblies ?? AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => !a.IsDynamic)
            .Where(x => !x.FullName.StartsWith("Microsoft"))
            .Where(x => !x.FullName.StartsWith("System"))
            .Where(x => !x.FullName.StartsWith("netstandard"))
            .Where(x => !x.FullName.StartsWith("JetBrains"))
            .Where(x => !x.FullName.StartsWith("xunit"));

        object[] parameters = [services];

        foreach (Assembly assembly in assemblies)
        {
            var types = assembly.GetTypes().Where(type => 
                RegistryType.IsAssignableFrom(type) && type != RegistryType)
                ;
            
            foreach (Type type in types)
            {
                InvokeTargetMethod(type, parameters);
            }
        }
        // todo emit warning (how?) if not "exactly one" type is found 
    }

    private static bool InvokeTargetMethod(Type contextType, object[] parameters)
    {
        if (contextType == null) return true;

        var method = contextType.GetMethod(nameof(IPipelineRegistry.Apply));

        if (method == null) return true;

        object context = Activator.CreateInstance(contextType);

        method.Invoke(context, parameters);

        return false;
    }
}