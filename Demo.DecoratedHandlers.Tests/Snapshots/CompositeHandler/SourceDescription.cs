using Demo.DecoratedHandlers.Tests.Models;
using Demo.DecoratedHandlers.Tests.Snapshots.OneBehavior;

namespace Demo.DecoratedHandlers.Tests.Snapshots.CompositeHandler;

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
        Behaviors.Add(new(nameof(LogBehavior<string, string>)));

        SourceFiles.Add(DefaultSourceFile);
        ExpectedFiles.Add(new FileDescription("ExpectedAlpha.cs", "BarHandler_Pipeline.g.cs"));
        ExpectedFiles.Add(new FileDescription("ExpectedBeta.cs", "BarHandler_Pipeline_1.g.cs"));
    }
}