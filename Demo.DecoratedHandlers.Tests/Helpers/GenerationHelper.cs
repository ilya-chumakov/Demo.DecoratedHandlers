﻿using System.Text;
using Demo.DecoratedHandlers.Gen;
using Demo.DecoratedHandlers.Tests.Models;
using Microsoft.CodeAnalysis.Text;

namespace Demo.DecoratedHandlers.Tests.Helpers;

public static class GenerationHelper
{
    public static async Task AssertGenerationEquality(
        List<TestFile> sourceFiles, 
        List<TestFile> expectedFiles)
    {
        var test = new Verifier.Test();
        foreach (TestFile file in sourceFiles)
        {
            test.TestState.Sources.Add((filename: "SomeUnusedName.cs", content: file.Content));
        }

        foreach (var result in expectedFiles)
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