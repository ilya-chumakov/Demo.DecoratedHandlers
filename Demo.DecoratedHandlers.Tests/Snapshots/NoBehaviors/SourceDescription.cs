using Demo.DecoratedHandlers.Tests.Models;

namespace Demo.DecoratedHandlers.Tests.Snapshots.NoBehaviors;

public class SourceDescription : SourceDescriptionBase
{
    public SourceDescription()
    {
        Handlers.Add(new(
            Name: nameof(BarHandler),
            FullName: GetDisplayFullName<BarHandler>(),
            ContainingNamespace: typeof(BarHandler).Namespace,
            InputFullName: nameof(Alpha),
            OutputFullName: nameof(Omega)));
            
        SourceFiles.Add(DefaultSourceFile);
    }
}