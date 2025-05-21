using System.Text;
using Demo.DecoratedHandlers.Gen;
using Demo.DecoratedHandlers.Tests.Text;
using Microsoft.CodeAnalysis.Text;
using Xunit.Abstractions;

namespace Demo.DecoratedHandlers.Tests.Roslyn;

public class FullTests(ITestOutputHelper output)
{
    private static readonly string GeneratorAssemblyVersion = 
        typeof(TextEmitter).Assembly.GetName().Version?.ToString();

    [Theory]
    [InlineData("TwoBehaviors")]
    public async Task GeneratorOutput_Default_OK(string snapshotName)
    {
        string source = await ReadSnapshotAsync(snapshotName, "Source.cs");
        string generated = await ReadSnapshotAsync(snapshotName, "Generated.cs");

        string expected = LineEndingsHelper.Normalize(generated)
            .Replace("%VERSION%", GeneratorAssemblyVersion);

        await new Verifier.Test
        {
            TestState =
            {
                Sources =
                {
                    (filename: "SomeUnusedName.cs", content: source)
                },
                GeneratedSources =
                {
                    (
                        sourceGeneratorType: typeof(PipelineGenerator),
                        filename: "BarHandler_Pipeline.g.cs",
                        content: SourceText.From(expected, Encoding.UTF8)
                    )
                }
            }
        }.RunAsync();
    }

    [Theory]
    [InlineData("NoBehaviors")]
    public async Task GeneratorOutput_ValidCaseOfNoGeneration_OK(string snapshotName)
    {
        string source = await ReadSnapshotAsync(snapshotName, "Source.cs");

        await new Verifier.Test
        {
            TestState =
            {
                Sources =
                {
                    (filename: "SomeUnusedName.cs", content: source)
                }
            }
        }.RunAsync();
    }

    private static async Task<string> ReadSnapshotAsync(string snapshot, string file)
    {
        string path = Path.Combine("Roslyn\\Snapshots\\" + snapshot, file);

        return await File.ReadAllTextAsync(path);
    }
}