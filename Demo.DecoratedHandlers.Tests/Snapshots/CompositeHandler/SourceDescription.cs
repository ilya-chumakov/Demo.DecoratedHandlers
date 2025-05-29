using Demo.DecoratedHandlers.Tests.Models;

namespace Demo.DecoratedHandlers.Tests.Snapshots.CompositeHandler;

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

        SourceFiles.Add(DefaultSourceFile);
        ExpectedFiles.Add(new FileDescription("ExpectedAlpha.cs", "BarHandler_Pipeline.g.cs"));
        ExpectedFiles.Add(new FileDescription("ExpectedBeta.cs", "BarHandler_Pipeline_1.g.cs"));
    }
}