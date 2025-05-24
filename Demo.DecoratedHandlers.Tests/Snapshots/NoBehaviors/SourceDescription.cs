using Demo.DecoratedHandlers.Gen;
using Demo.DecoratedHandlers.Tests.Helpers;

namespace Demo.DecoratedHandlers.Tests.Snapshots.NoBehaviors;

public class SourceDescription : ISourceDescription
{
    public string FolderName { get; init; } = nameof(NoBehaviors);

    public List<HandlerDescription> Handlers { get; init; } =
    [
        new(
            HandlerTypeName: nameof(NoBehaviors.BarHandler),
            InputTypeName: nameof(NoBehaviors.Alpha),
            OutputTypeName: nameof(NoBehaviors.Omega),
            OutputNamespace: typeof(NoBehaviors.Alpha).Namespace
        )
    ];

    public List<BehaviorDescription> Behaviors { get; init; } =
    [
    ];
}