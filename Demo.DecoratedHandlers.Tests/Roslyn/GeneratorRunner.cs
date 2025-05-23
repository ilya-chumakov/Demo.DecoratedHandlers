using System.Runtime;
using Demo.DecoratedHandlers.Abstractions;
using Demo.DecoratedHandlers.Gen;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Demo.DecoratedHandlers.Tests.Roslyn;

public static class GeneratorRunner
{
    public static GeneratorDriverRunResult GetRunResult(string source)
    {
        var generator = new PipelineGenerator();
        var compilation = CSharpCompilation.Create("TestAssembly",
            syntaxTrees: [CSharpSyntaxTree.ParseText(source)],
            references:
            [
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(AssemblyTargetedPatchBandAttribute).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(IRequestHandler<,>).Assembly.Location)
            ],
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
        );

        return CSharpGeneratorDriver.Create(generator).RunGenerators(compilation).GetRunResult();
    }
}