using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
// ReSharper disable RedundantLambdaParameterType

namespace Demo.DecoratedHandlers.Gen;

[Generator]
public class GenericGenerator : IIncrementalGenerator
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
                predicate: static (SyntaxNode node, CancellationToken ct) => node is ClassDeclarationSyntax cds,
                transform: static (GeneratorSyntaxContext ctx, CancellationToken ct) =>
                {
                    var declaration = (ClassDeclarationSyntax)ctx.Node;
                    var model = ctx.SemanticModel;
                    var symbol = model.GetDeclaredSymbol(declaration);

                    if (symbol is { IsGenericType: false } &&
                        symbol.Interfaces.Any(i => i.Name == "IGenericHandler") &&
                        symbol.GetAttributes()
                            .Any(attr => attr.AttributeClass?.Name == nameof(DecorateThisHandler)))
                    {
                        return new HandlerData(
                            symbol.Name,
                            symbol.Interfaces.First().TypeArguments.First().Name!,
                            symbol.ContainingNamespace.ToDisplayString()
                        );
                    }
                    return default;
                })
            .Where(static symbol => symbol != default)
            .Collect();

        var decorators = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (SyntaxNode node, CancellationToken ct) => node is ClassDeclarationSyntax cds,
                transform: static (GeneratorSyntaxContext ctx, CancellationToken ct) =>
                {
                    var declaration = (ClassDeclarationSyntax)ctx.Node;
                    var model = ctx.SemanticModel;
                    var symbol = model.GetDeclaredSymbol(declaration);

                    if (symbol is { IsGenericType: false } &&
                        symbol.GetAttributes().Any(attr => attr.AttributeClass?.Name == nameof(UseThisDecorator)))
                    {
                        return new DecoratorData(symbol.Name);
                    }
                    return default;
                })
            .Where(static symbol => symbol != default)
            .Collect();

        var combination = handlers.Combine(decorators);

        //todo RegisterImplementationSourceOutput?
        context.RegisterSourceOutput(combination, static (ctx, symbols) =>
        {
            var (handlers, decorators) = symbols;

            foreach (var handler in handlers)
            {
                ctx.AddSource($"{handler.HandlerTypeName}_Pipeline.g.cs", TextBuilder.CreatePipeline(handler, decorators));
            }
        });
    }
}