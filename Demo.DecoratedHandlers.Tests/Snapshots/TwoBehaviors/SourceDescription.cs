using Demo.DecoratedHandlers.Tests.Models;

namespace Demo.DecoratedHandlers.Tests.Snapshots.TwoBehaviors;

public class SourceDescription : SourceDescriptionBase
{
    public SourceDescription()
    {
        Handlers.Add(new(
            HandlerTypeName: nameof(BarHandler),
            HandlerTypeFullName: "global::" + typeof(BarHandler).FullName,
            InputTypeName: nameof(Alpha),
            OutputTypeName: nameof(Omega),
            ContainingNamespace: typeof(Alpha).Namespace
        ));
        Behaviors.Add(new(nameof(LogBehavior<string, string>)));
        Behaviors.Add(new(nameof(ExceptionBehavior<string, string>)));

        SourceFiles.Add(DefaultSourceFile);
        ExpectedFiles.Add(DefaultExpectedFile);
    }
}