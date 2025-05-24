using System.Text;
using Demo.DecoratedHandlers.Gen;
using Microsoft.CodeAnalysis.Text;

namespace Demo.DecoratedHandlers.Tests.Helpers;

public static class GenerationHelper
{
    public static async Task AssertGenerationEquality(TestDescription description)
    {
        var test = new Verifier.Test();
        test.TestState.Sources.Add((filename: "SomeUnusedName.cs", content: description.Source.Content));

        foreach (var result in description.Results)
        {
            test.TestState.GeneratedSources.Add((
                sourceGeneratorType: typeof(PipelineGenerator),
                filename: result.Name,
                content: SourceText.From(result.Content, Encoding.UTF8)
            ));
        }
        await test.RunAsync();
    }
}