using Demo.DecoratedHandlers.Tests.Helpers;
using Xunit.Abstractions;

namespace Demo.DecoratedHandlers.Tests;

public class PipelineGeneratorTests(ITestOutputHelper output)
{
    [Fact]
    public async Task GeneratorOutput_NoBehavior_OK()
    {
        var sourceDescription = new Snapshots.NoBehaviors.SourceDescription();
        var test = new TestDescription
        {
            SourceDescription = sourceDescription,
            Source = await SnapshotReader.ReadSourceAsync(sourceDescription.FolderName),
            Results = []
        };

        await AssertGen(test, false);
    }

    [Fact]
    public async Task GeneratorOutput_OneBehavior_OK()
    {
        var sourceDescription = new Snapshots.OneBehavior.SourceDescription();
        var test = new TestDescription
        {
            SourceDescription = sourceDescription,
            Source = await SnapshotReader.ReadSourceAsync(sourceDescription.FolderName),
            Results = [await SnapshotReader.ReadExpectedAsync(sourceDescription.FolderName)]
        };

        await AssertGen(test, false);
    }

    [Fact]
    public async Task GeneratorOutput_TwoBehaviors_OK()
    {
        var sourceDescription = new Snapshots.TwoBehaviors.SourceDescription();
        var test = new TestDescription
        {
            SourceDescription = sourceDescription,
            Source = await SnapshotReader.ReadSourceAsync(sourceDescription.FolderName),
            Results = [await SnapshotReader.ReadExpectedAsync(sourceDescription.FolderName)]
        };

        await AssertGen(test, false);
    }

    private static async Task AssertGen(TestDescription test, bool requireResult = true)
    {
        // text
        if (requireResult && test.Results.Count == 0)
        {
            Assert.Fail("expected at least snapshot result");
        }

        if (test.Results.Count > 0)
        {
            TextHelper.AssertEquality(test.SourceDescription, test.Results.Single().Content);
        }

        // compilation
        CompilationHelper.AssertCompilation(test.Source.Content);

        // generation
        await GenerationHelper.AssertGenerationEquality(test);
    }
}