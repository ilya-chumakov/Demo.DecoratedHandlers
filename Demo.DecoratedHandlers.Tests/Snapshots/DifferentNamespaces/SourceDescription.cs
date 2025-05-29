using Demo.DecoratedHandlers.Tests.Models;
using Demo.DecoratedHandlers.Tests.Snapshots.DifferentNamespaces.RequestNamespace;
using Demo.DecoratedHandlers.Tests.Snapshots.DifferentNamespaces.ResponseNamespace;
using Demo.DecoratedHandlers.Tests.Snapshots.DifferentNamespaces.HandlerNamespace;
using Demo.DecoratedHandlers.Tests.Snapshots.DifferentNamespaces.BehaviorNamespace;

namespace Demo.DecoratedHandlers.Tests.Snapshots.DifferentNamespaces;

public class SourceDescription : SourceDescriptionBase
{
    public SourceDescription()
    {
        Handlers.Add(new(
            HandlerTypeName: nameof(BarHandler),
            HandlerTypeFullName:  "global::" + typeof(BarHandler).FullName,
            InputTypeName: nameof(Alpha),
            OutputTypeName: nameof(Omega),
            ContainingNamespace: typeof(Alpha).Namespace
        ));
        Behaviors.Add(new(nameof(LogBehavior<string, string>)));

        SourceFiles.Add(DefaultSourceFile);
        ExpectedFiles.Add(DefaultExpectedFile);
    }
}