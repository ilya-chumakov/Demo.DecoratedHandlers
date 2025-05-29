using Demo.DecoratedHandlers.Gen;
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
            Name: nameof(BarHandler),
            FullName: GetDisplayFullName<BarHandler>(),
            ContainingNamespace: typeof(Alpha).Namespace,
            InputFullName: GetDisplayFullName<Alpha>(),
            OutputFullName: GetDisplayFullName<Omega>()));

        Behaviors.Add(new BehaviorDescription(
                GetDisplayFullName<LogBehavior<string, string>>()
            )
        );

        SourceFiles.Add(DefaultSourceFile);
        ExpectedFiles.Add(DefaultExpectedFile);
    }
}