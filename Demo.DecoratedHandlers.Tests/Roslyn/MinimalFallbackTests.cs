using System.Runtime;
using Demo.DecoratedHandlers.Abstractions;
using Demo.DecoratedHandlers.Gen;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Xunit.Abstractions;

namespace Demo.DecoratedHandlers.Tests.Roslyn;

/// <summary>
///     Simple generator driver tests.
///     Just in case if something goes wrong with the full Roslyn tests.
/// </summary>
public class MinimalFallbackTests(ITestOutputHelper output)
{
    private static GeneratorDriver CreateDriver(string source)
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

        return CSharpGeneratorDriver.Create(generator).RunGenerators(compilation);
    }

    [Fact]
    public void GeneratesMarkerForClassImplementingTargetInterface()
    {
        const string source = @"
using Demo.DecoratedHandlers.Abstractions;

namespace MyNamespace;

public record Alpha;
public record Omega;

public class Bar : IRequestHandler<Alpha, Omega> { }

public class LogBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    public Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}
";

        var driver = CreateDriver(source);
        var runResult = driver.GetRunResult();

        var generated = runResult.Results[0].GeneratedSources;

        Assert.Single(generated);
        Assert.Contains("Bar_Pipeline.g.cs", generated[0].HintName);

        string actual = generated[0].SourceText.ToString();
        output.WriteLine(actual);
        Assert.Contains("BarPipeline(IServiceProvider provider) : IRequestHandler<Alpha, Omega>", actual);
    }

    [Fact]
    public void DoesNotGenerateForClassWithoutMatchingInterface()
    {
        // todo arrange interface with the same name but different namespace, assert it's not invoked
        const string source = @"
namespace MyNamespace;

public record Alpha;
public record Omega;

public interface IRequestHandler<TInput, TOutput> { }

public class Bar : MyNamespace.IRequestHandler<Alpha, Omega> { }
";
        var driver = CreateDriver(source);
        var runResult = driver.GetRunResult();

        Assert.Empty(runResult.Results[0].GeneratedSources);
    }
}