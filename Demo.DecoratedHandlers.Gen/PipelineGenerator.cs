using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
// ReSharper disable PreferConcreteValueOverDefault
// ReSharper disable RedundantLambdaParameterType
#pragma warning disable IDE0305

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

            // distinct to support partial declarations
            behaviors = behaviors.Distinct().ToImmutableArray();
            handlers = handlers.Distinct().ToImmutableArray();

            var pipelines = new List<PipelineDescription>(handlers.Length);
            foreach (HandlerDescription handler in handlers) 
            {
                string filename = $"{handler.Name}_Pipeline{handler.PipelineSuffix}.g.cs";
                (SourceText text, PipelineDescription pd) = PipelineTextEmitter.CreateSourceText(handler, behaviors);
                ctx.AddSource(filename, text);

                pipelines.Add(pd);
            }

            SourceText registration = RegistryTextEmitter.CreateSourceText(pipelines);
            ctx.AddSource("PipelineRegistry.g.cs", registration);
        });
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

            // GetTypeByMetadataName call to get interface symbol for filtering
            // maybe less efficient since it may perform full assembly scan

            IEnumerable<INamedTypeSymbol> interfaces = handler.AllInterfaces.Where(i =>
                i.ContainingAssembly.Name == AbstractionsMetadata.AssemblySymbolName &&
                i.Name == AbstractionsMetadata.RequestInterfaceSymbolName);

            List<HandlerDescription> descriptions = [];

            // todo ensure stable order only on debug/testing
            interfaces = interfaces.OrderBy(x => x.TypeArguments[0].Name!);

            int index = 0;
            foreach (INamedTypeSymbol interf in interfaces)
            {
                string pipelineSuffix = index == 0 ? string.Empty : '_' + index.ToString();
                index++;

                // FYI: use Name property for short name
                // todo do we really need to call ToDisplayString?

                var description = new HandlerDescription(
                    Name: handler.Name,
                    FullName: handler.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
                    ContainingNamespace: handler.ContainingNamespace.ToDisplayString(),
                    InputFullName: interf.TypeArguments[0].ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat), 
                    OutputFullName: interf.TypeArguments[1].ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat), 
                    PipelineSuffix: pipelineSuffix);
                descriptions.Add(description);
            }
            return descriptions;
        };
    }

    private static Func<GeneratorSyntaxContext, CancellationToken, BehaviorDescription> TransformBehaviors()
    {
        return static (GeneratorSyntaxContext ctx, CancellationToken _) =>
        {
            if (ctx.Node is not ClassDeclarationSyntax syntax || syntax.BaseList is null)
                return default;

            INamedTypeSymbol behavior = ctx.SemanticModel.GetDeclaredSymbol(syntax);

            if (behavior is not { IsGenericType: true, IsAnonymousType: false } ||
                behavior.AllInterfaces is not { Length: > 0 })
                return default;

            INamedTypeSymbol interf = behavior.AllInterfaces.FirstOrDefault(i =>
                i.ContainingAssembly.Name == AbstractionsMetadata.AssemblySymbolName &&
                i.Name == AbstractionsMetadata.BehaviorInterfaceSymbolName);

            if (interf is null) return default;

            string ns = behavior.ContainingNamespace.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            
            return new BehaviorDescription(
                FullNamePrefix: ns + '.' + behavior.Name
            );
        };
    }

}