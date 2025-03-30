﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Demo.DecoratedHandlers.Gen
{
    [Generator]
    public class GenericGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            // Find handlers with the attribute
            var provider = context.SyntaxProvider
                .CreateSyntaxProvider(
                    predicate: (node, _) => node is ClassDeclarationSyntax,
                    transform: (ctx, _) => GetClassSymbolIfDecorated(ctx)
                )
                .Where(symbol => symbol != null)
                .Collect();

            context.RegisterSourceOutput(provider, (ctx, symbols) =>
            {
                var globalDecorators = symbols.Where(x =>
                        x.GetAttributes().Any(attr => attr.AttributeClass?.Name == nameof(UseThisDecorator)))
                    .ToList();

                var handlers = symbols.Where(x =>
                    !x.IsGenericType &&
                    // todo solve dependency
                    //x.Interfaces.Any(i => i.Name == nameof(IConcreteHandler)) &&
                    x.Interfaces.Any(i => i.Name == "IGenericHandler") &&
                    x.GetAttributes().Any(attr => attr.AttributeClass?.Name == nameof(DecorateThisHandler))
                );

                foreach (INamedTypeSymbol handler in handlers)
                {
                    ctx.AddSource($"{handler.Name}Sequence.g.cs", GenerateDecorator(handler, globalDecorators));
                }
            });
        }

        private static INamedTypeSymbol GetClassSymbolIfDecorated(GeneratorSyntaxContext context)
        {
            var declaration = (ClassDeclarationSyntax)context.Node;
            var model = context.SemanticModel;
            var symbol = model.GetDeclaredSymbol(declaration);

            if (symbol?.GetAttributes().Any(attr =>
                    attr.AttributeClass?.Name == nameof(DecorateThisHandler)
                    || attr.AttributeClass?.Name == nameof(UseThisDecorator)
                ) == true)
            {
                return symbol;
            }

            return null;
        }

        private static SourceText GenerateDecorator(INamedTypeSymbol classSymbol,
            List<INamedTypeSymbol> globalDecorators)
        {
            var intr = classSymbol.Interfaces.First();
            var inputType = intr.TypeArguments.First();
            string inputTypeName = inputType.Name;
            string inputTypeFullNamespace = inputType.ContainingNamespace.Name;

            string namespaceName = classSymbol.ContainingNamespace.ToDisplayString();
            string handlerName = classSymbol.Name;
            string className = $"{handlerName}Sequence";
            string targetFunc = "hf";

            var sb = new StringBuilder();
            sb.Append($@"
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Demo.DecoratedHandlers.Gen;
    // using {inputTypeFullNamespace};

    // <auto-generated by {nameof(GenericGenerator)}/> 
    namespace {namespaceName} 
    {{ 
        public class {className}(IServiceProvider provider) : IGenericHandler<{inputTypeName}>
        {{ 
		    public Task HandleAsync({inputTypeName} input) 
		    {{
			    var handler = provider.GetRequiredService<{handlerName}>(); 
			    var {targetFunc} = () => handler.HandleAsync(input);
    ");

            for (int i = 0; i < globalDecorators.Count; i++)
            {
                string currentFunc = $"df{i}";
                string currentDecorator = $"d{i}";

                sb.Append($@"
                var {currentDecorator} = provider.GetRequiredService<{globalDecorators[i].Name}>();
                var {currentFunc} = () => {currentDecorator}.HandleAsync({targetFunc});
        ");
                targetFunc = currentFunc;
            }

            sb.Append($@"
			    return {targetFunc}();
		    }}
        }} 

        public static class ServiceCollectionExtensions_{className}
        {{
            [RegisterThis]
            public static void RegisterSequence(this IServiceCollection services)
            {{
                services.RemoveAll<IGenericHandler<{inputTypeName}>>();
                services.AddTransient<IGenericHandler<{inputTypeName}>, {className}>();
                services.AddTransient<{handlerName}>();
            }}
        }}
    }} 
        ");
            return SourceText.From(sb.ToString(), Encoding.UTF8);
        }
    }
}