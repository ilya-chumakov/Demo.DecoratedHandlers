using System;
using System.Collections.Generic;
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
            .SelectMany(static (symbol, _) => symbol) // flatten
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
                string filename = $"{handler.HandlerTypeName}_Pipeline{handler.PipelineSuffix}.g.cs";
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

            if (symbol is not { IsGenericType: true, IsAnonymousType: false } ||
                symbol.AllInterfaces is not { Length: > 0 })
                return default;

            var face = symbol.AllInterfaces.FirstOrDefault(i =>
                i.ContainingAssembly.Name == AbstractionsMetadata.AssemblySymbolName &&
                i.Name == AbstractionsMetadata.BehaviorInterfaceSymbolName);

            if (face is null) return default;

            return new BehaviorDescription(symbol.Name);
        };
    }

    private static Func<GeneratorSyntaxContext, CancellationToken, IEnumerable<HandlerDescription>> TransformHandlers()
    {
        return static (GeneratorSyntaxContext ctx, CancellationToken _) =>
        {
            if (ctx.Node is not ClassDeclarationSyntax syntax || syntax.BaseList is null)
                return default;

            INamedTypeSymbol handler = ctx.SemanticModel.GetDeclaredSymbol(syntax);

            if (handler is not { IsGenericType: false, IsAnonymousType: false } ||
                handler.AllInterfaces is not { Length: > 0 })
                return default;

            IEnumerable<INamedTypeSymbol> interfaces = handler.AllInterfaces.Where(i =>
                i.ContainingAssembly.Name == AbstractionsMetadata.AssemblySymbolName &&
                i.Name == AbstractionsMetadata.RequestInterfaceSymbolName);

            List<HandlerDescription> descriptions = [];

            // todo ensure stable order only on debug/testing
            interfaces = interfaces.OrderBy(x => x.TypeArguments[0].Name!);

            int index = 0;
            foreach (var interf in interfaces)
            {
                string pipelineSuffix = index == 0 ? string.Empty : '_' + index.ToString();
                index++;

                var description = new HandlerDescription(
                    HandlerTypeName: handler.Name,
                    InputTypeName: interf.TypeArguments[0].Name!,
                    OutputTypeName: interf.TypeArguments[1].Name!,
                    // todo do we really need to call ToDisplayString?
                    OutputNamespace: handler.ContainingNamespace.ToDisplayString(),
                    PipelineSuffix: pipelineSuffix
                );
                descriptions.Add(description);
            }
            return descriptions;
        };
    }
}