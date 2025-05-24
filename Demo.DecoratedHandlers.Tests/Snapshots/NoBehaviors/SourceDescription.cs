using Demo.DecoratedHandlers.Tests.Models;

namespace Demo.DecoratedHandlers.Tests.Snapshots.NoBehaviors;

public class SourceDescription : SourceDescriptionBase
{
    public SourceDescription()
    {
        Handlers.Add(new(
            HandlerTypeName: nameof(BarHandler),
            InputTypeName: nameof(Alpha),
            OutputTypeName: nameof(Omega),
            OutputNamespace: typeof(Alpha).Namespace
        ));

        SourceFiles.Add(DefaultSourceFile);
    }
}