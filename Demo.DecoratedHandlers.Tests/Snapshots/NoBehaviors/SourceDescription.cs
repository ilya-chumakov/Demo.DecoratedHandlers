using Demo.DecoratedHandlers.Tests.Models;

namespace Demo.DecoratedHandlers.Tests.Snapshots.NoBehaviors;

public class SourceDescription : SourceDescriptionBase
{
    public SourceDescription()
    {
        Handlers.Add(new(
            Name: nameof(BarHandler),
            FullName: "global::" + typeof(BarHandler).FullName,
            ContainingNamespace: typeof(Alpha).Namespace, InputFullName: nameof(Alpha), OutputFullName: nameof(Omega)));

        SourceFiles.Add(DefaultSourceFile);
    }
}