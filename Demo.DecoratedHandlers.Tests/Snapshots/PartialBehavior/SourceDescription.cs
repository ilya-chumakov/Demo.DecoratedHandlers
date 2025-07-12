using Demo.DecoratedHandlers.Gen;
using Demo.DecoratedHandlers.Tests.Models;

namespace Demo.DecoratedHandlers.Tests.Snapshots.PartialBehavior;

public class SourceDescription : SourceDescriptionBase
{
    public SourceDescription()
    {
        Handlers.Add(new(
            Name: nameof(BarHandler),
            FullName: GetDisplayFullName<BarHandler>(),
            ContainingNamespace: typeof(BarHandler).Namespace,
            InputFullName: GetDisplayFullName<Alpha>(),
            OutputFullName: GetDisplayFullName<Omega>()));

        Behaviors.Add(new BehaviorDescription(
                GetDisplayFullName<LogBehavior<string, string>>()
            )
        );
        
        AddDefaultFiles();
    }
}