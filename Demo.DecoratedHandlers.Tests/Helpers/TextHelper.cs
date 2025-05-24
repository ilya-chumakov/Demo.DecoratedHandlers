using Demo.DecoratedHandlers.Gen;
using Microsoft.CodeAnalysis.Text;

namespace Demo.DecoratedHandlers.Tests.Helpers;

public class TextHelper
{
    public static void AssertEquality(ISourceDescription sourceDescription, string expected)
    {
        SourceText actual =
            TextEmitter.CreatePipelineText(sourceDescription.Handlers.Single(), sourceDescription.Behaviors);

        string[] expectedLines = LineEndingsHelper.Normalize(expected)
            .Replace("%VERSION%", typeof(TextEmitter).Assembly.GetName().Version?.ToString())
            .Split(Environment.NewLine);

        bool areEqual = RoslynTestUtils.CompareLines(expectedLines, actual, out string errorMessage);

        string output = errorMessage;

        if (!areEqual)
        {
            output += Environment.NewLine + Environment.NewLine + actual;
        }

        Assert.True(areEqual, output);
    }
}