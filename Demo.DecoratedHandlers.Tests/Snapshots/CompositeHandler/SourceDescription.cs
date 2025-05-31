using Demo.DecoratedHandlers.Gen;
using Demo.DecoratedHandlers.Tests.Models;

namespace Demo.DecoratedHandlers.Tests.Snapshots.CompositeHandler;

public class SourceDescription : SourceDescriptionBase
{
    public SourceDescription()
    {
        Handlers.Add(new(
            Name: nameof(BarHandler),
            FullName: GetDisplayFullName<BarHandler>(),
            ContainingNamespace: typeof(BarHandler).Namespace,
            InputFullName: GetDisplayFullName<Alpha>(),
            OutputFullName: GetDisplayFullName<Omega>()));
        
        Behaviors.Add(new BehaviorDescription(
                GetDisplayFullName<LogBehavior<string, string>>()
            )
        );

        SourceFiles.Add(DefaultSourceFile);
        ExpectedFiles.Add(new FileDescription("ExpectedAlpha.cs", "BarHandler_Pipeline.g.cs"));
        ExpectedFiles.Add(new FileDescription("ExpectedBeta.cs", "BarHandler_Pipeline_1.g.cs"));
    }
}