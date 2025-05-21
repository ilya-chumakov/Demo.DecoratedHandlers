using System;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
// ReSharper disable PreferConcreteValueOverDefault
// ReSharper disable RedundantLambdaParameterType

namespace Demo.DecoratedHandlers.Gen;

[Generator]
public class PipelineGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
#if LAUNCH_DEBUGGER
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                System.Diagnostics.Debugger.Launch();
            }
#endif
        var handlers = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (SyntaxNode node, CancellationToken _) => node is ClassDeclarationSyntax,
                transform: TransformHandlers())
            .Where(static symbol => symbol != default)
            .Collect();

        var behaviors = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (SyntaxNode node, CancellationToken _) => node is ClassDeclarationSyntax,
                transform: TransformBehaviors())
            .Where(static symbol => symbol != default)
            .Collect();

        var combination = handlers.Combine(behaviors);

        //todo RegisterImplementationSourceOutput?
        context.RegisterSourceOutput(combination, static (ctx, symbols) =>
        {
            var (handlers, behaviors) = symbols;

            // nothing to do now if no behaviors
            if (behaviors.Length == 0) return;

            //todo it's useful, make conditional? how?
            //ctx.AddSource("Stats.g.cs", DebugEmitter.CreateStatistics(handlers, behaviors));
                
            foreach (var handler in handlers)
            {
                string filename = $"{handler.HandlerTypeName}_Pipeline.g.cs";
                SourceText sourceText = TextEmitter.CreatePipelineText(handler, behaviors);
                ctx.AddSource(filename, sourceText);
            }
        });
    }

    private static Func<GeneratorSyntaxContext, CancellationToken, BehaviorDescription> TransformBehaviors()
    {
        return static (GeneratorSyntaxContext ctx, CancellationToken _) =>
        {
            if (ctx.Node is not ClassDeclarationSyntax syntax || syntax.BaseList is null)
                return default;

            var symbol = ctx.SemanticModel.GetDeclaredSymbol(syntax);

            if (symbol is not { IsGenericType: false, IsAnonymousType: false } ||
                symbol.AllInterfaces is not { Length: > 0 })
                return default;

            var face = symbol.AllInterfaces.FirstOrDefault(i => 
                i.ContainingAssembly.Name == AbstractionsMetadata.AssemblySymbolName &&
                i.Name == AbstractionsMetadata.BehaviorInterfaceSymbolName);

            if (face is null) return default;

            return new BehaviorDescription(symbol.Name);
        };
    }

    private static Func<GeneratorSyntaxContext, CancellationToken, HandlerDescription> TransformHandlers()
    {
        return static (GeneratorSyntaxContext ctx, CancellationToken _) =>
        {
            if (ctx.Node is not ClassDeclarationSyntax syntax || syntax.BaseList is null)
                return default;

            var symbol = ctx.SemanticModel.GetDeclaredSymbol(syntax);

            if (symbol is not { IsGenericType: false, IsAnonymousType: false } ||
                symbol.AllInterfaces is not { Length: > 0 })
                return default;

            var face = symbol.AllInterfaces.FirstOrDefault(i => 
                i.ContainingAssembly.Name == AbstractionsMetadata.AssemblySymbolName &&
                i.Name == AbstractionsMetadata.RequestInterfaceSymbolName);

            if (face is null) return default;

            return new HandlerDescription(
                symbol.Name,
                face.TypeArguments[0].Name!,
                face.TypeArguments[1].Name!,
                // todo do we really need to call ToDisplayString?
                symbol.ContainingNamespace.ToDisplayString()
            );
        };
    }
}