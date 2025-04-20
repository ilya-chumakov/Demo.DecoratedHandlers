using System.Runtime;
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
            [CSharpSyntaxTree.ParseText(source)],
            [
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(AssemblyTargetedPatchBandAttribute).Assembly.Location)
            ],
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
        );

        return CSharpGeneratorDriver.Create(generator).RunGenerators(compilation);
    }

    [Fact]
    public void GeneratesMarkerForClassImplementingTargetInterface()
    {
        const string source = @"
namespace MyNamespace;

public record Alpha;
public record Omega;

public interface IGenericHandler<TInput, TOutput> { }

[Demo.DecoratedHandlers.Abstractions.DecorateThisHandler]
public class Bar : IGenericHandler<Alpha, Omega> { }
";

        var driver = CreateDriver(source);
        var runResult = driver.GetRunResult();

        var generated = runResult.Results[0].GeneratedSources;

        Assert.Single(generated);
        Assert.Contains("Bar_Pipeline.g.cs", generated[0].HintName);

        string actual = generated[0].SourceText.ToString();
        output.WriteLine(actual);
        Assert.Contains("BarPipeline(IServiceProvider provider) : IGenericHandler<Alpha, Omega>", actual);
    }

    [Fact]
    public void DoesNotGenerateForClassWithoutMatchingInterface()
    {
        // todo arrange interface with the same name but different namespace, assert it's not invoked
        const string source = @"
namespace MyNamespace;

public record Alpha;
public record Omega;

public interface IGenericHandler<TInput, TOutput> { }

//[Demo.DecoratedHandlers.Abstractions.DecorateThisHandler]
public class Bar : IGenericHandler<Alpha, Omega> { }
";
        var driver = CreateDriver(source);
        var runResult = driver.GetRunResult();

        Assert.Empty(runResult.Results[0].GeneratedSources);
    }
}