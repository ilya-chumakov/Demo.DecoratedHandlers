using Demo.DecoratedHandlers.Tests.Models;

namespace Demo.DecoratedHandlers.Tests.Snapshots.NoBehaviors;

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

        SourceFiles.Add(DefaultSourceFile);
    }
}