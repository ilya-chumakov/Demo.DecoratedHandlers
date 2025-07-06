using Demo.DecoratedHandlers.Gen;
using DiffPlex.Renderer;
using Microsoft.CodeAnalysis.Text;

namespace Demo.DecoratedHandlers.Tests.Helpers;

public class TextHelper
{
    public static void AssertEquality(string expected, HandlerDescription handler, List<BehaviorDescription> behaviors)
    {
        SourceText actual = TextEmitter.CreatePipelineText(handler, behaviors);

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
    public static void AssertEqualityWithDiffPlex(string expected, HandlerDescription handler, List<BehaviorDescription> behaviors)
    {
        string actual = TextEmitter.CreatePipelineText(handler, behaviors).ToString();

        string expectedNormalized = LineEndingsHelper.Normalize(expected)
            .Replace("%VERSION%", typeof(TextEmitter).Assembly.GetName().Version?.ToString());

        if (actual == expectedNormalized) return;

        string unidiff = UnidiffRenderer.GenerateUnidiff(
            oldText: expectedNormalized, 
            newText: actual,
            contextLines: 3
        );

        Assert.Fail(unidiff);
    }
}