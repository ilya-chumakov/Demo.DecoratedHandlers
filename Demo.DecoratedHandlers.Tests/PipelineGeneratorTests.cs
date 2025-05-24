using Demo.DecoratedHandlers.Tests.Helpers;
using Demo.DecoratedHandlers.Tests.Models;
using Xunit.Abstractions;

namespace Demo.DecoratedHandlers.Tests;

public class PipelineGeneratorTests(ITestOutputHelper output)
{
    [Fact]
    public async Task GeneratorOutput_NoBehavior_OK()
    {
        await VerifyGenerationFrom<Snapshots.NoBehaviors.SourceDescription>();
    }

    [Fact]
    public async Task GeneratorOutput_OneBehavior_OK()
    {
        await VerifyGenerationFrom<Snapshots.OneBehavior.SourceDescription>();
    }

    [Fact]
    public async Task GeneratorOutput_TwoBehaviors_OK()
    {
        await VerifyGenerationFrom<Snapshots.TwoBehaviors.SourceDescription>();
    }

    private static async Task VerifyGenerationFrom<TSourceDescription>()
        where TSourceDescription : SourceDescriptionBase, new()
    {
        SourceDescriptionBase description = new TSourceDescription();

        var sourceFiles = await SnapshotReader.ReadAsync(description.FolderName, description.SourceFiles);
        var expectedFiles = await SnapshotReader.ReadAsync(description.FolderName, description.ExpectedFiles);

        // text
        if (expectedFiles.Count > 0)
        {
            TextHelper.AssertEquality(description, expectedFiles.Single().Content);
        }

        // compilation
        CompilationHelper.AssertCompilation(sourceFiles.Single().Content);

        // generation
        await GenerationHelper.AssertGenerationEquality(sourceFiles, expectedFiles);
    }
}