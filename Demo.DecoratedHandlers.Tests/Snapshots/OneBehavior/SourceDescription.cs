using Demo.DecoratedHandlers.Gen;
using Demo.DecoratedHandlers.Tests.Helpers;

namespace Demo.DecoratedHandlers.Tests.Snapshots.OneBehavior;

public class SourceDescription : ISourceDescription
{
    public string FolderName { get; init; } = nameof(OneBehavior);

    public List<HandlerDescription> Handlers { get; init; } =
    [
        new(
            HandlerTypeName: nameof(BarHandler),
            InputTypeName: nameof(Alpha),
            OutputTypeName: nameof(Omega),
            OutputNamespace: typeof(Alpha).Namespace
        )
    ];

    public List<BehaviorDescription> Behaviors { get; init; } =
    [
        new(nameof(LogBehavior<string, string>))
    ];
}