using System.Text;
using Demo.DecoratedHandlers.Gen;
using Microsoft.CodeAnalysis.Text;
using Xunit.Abstractions;
using VerifyCS = Demo.DecoratedHandlers.Tests.Roslyn.CSharpSourceGeneratorVerifier<Demo.DecoratedHandlers.Gen.PipelineGenerator>;

namespace Demo.DecoratedHandlers.Tests.Roslyn;

public class FullTests(ITestOutputHelper output)
{
    [Fact]
    public async Task DraftWithMsPackage()
    {
        string sourceCode = await File.ReadAllTextAsync(Path.Combine("Roslyn\\Snapshots", "MinV1.source.cs"));
        string expected = await File.ReadAllTextAsync(Path.Combine("Roslyn\\Snapshots", "MinV1.generated.cs"));

        expected = expected
            .Replace("%VERSION%", typeof(TextEmitter).Assembly.GetName().Version?.ToString());

        await new VerifyCS.Test
        {
            TestState =
            {
                Sources =
                {
                    (filename: "SomeUnusedName.cs", content: sourceCode)
                },
                GeneratedSources =
                {
                    (
                        sourceGeneratorType: typeof(PipelineGenerator),
                        filename: "Bar_Pipeline.g.cs",
                        content: SourceText.From(expected, Encoding.UTF8)
                    )
                }
            }
        }.RunAsync();
    }
}