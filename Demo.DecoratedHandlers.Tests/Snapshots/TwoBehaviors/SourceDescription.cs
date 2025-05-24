using Demo.DecoratedHandlers.Gen;
using Demo.DecoratedHandlers.Tests.Helpers;

namespace Demo.DecoratedHandlers.Tests.Snapshots.TwoBehaviors;

public class SourceDescription : ISourceDescription
{
    public string FolderName { get; init; } = nameof(TwoBehaviors);

    public List<HandlerDescription> Handlers { get; init; } =
    [
        new(
            HandlerTypeName: nameof(TwoBehaviors.BarHandler),
            InputTypeName: nameof(TwoBehaviors.Alpha),
            OutputTypeName: nameof(TwoBehaviors.Omega),
            OutputNamespace: typeof(TwoBehaviors.Alpha).Namespace
        )
    ];

    public List<BehaviorDescription> Behaviors { get; init; } =
    [
        new(nameof(TwoBehaviors.LogBehavior<string, string>)),
        new(nameof(TwoBehaviors.ExceptionBehavior<string, string>))
    ];
}