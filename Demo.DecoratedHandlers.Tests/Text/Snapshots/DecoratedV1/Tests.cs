using Demo.DecoratedHandlers.Gen;
using Microsoft.CodeAnalysis.Text;
using Xunit.Abstractions;

namespace Demo.DecoratedHandlers.Tests.Text.Snapshots.DecoratedV1;

public class Tests(ITestOutputHelper output)
{
    [Fact]
    public async Task Test_Baseline()
    {
        SourceText actual = TextEmitter.CreatePipelineText(
            new HandlerDescription(
                HandlerTypeName: nameof(FooHandler),
                InputTypeName: nameof(Alpha),
                OutputTypeName: nameof(Omega),
                OutputNamespace: typeof(Alpha).Namespace
                ),
            [
                new BehaviorDescription(nameof(Bv1<string,string>))
            ]
        );

        string path = Path.Combine("Text\\Snapshots\\DecoratedV1", "Generated.cs");

        await VerifyAgainstBaselineUsingFile(path, actual);
    }

    private async Task VerifyAgainstBaselineUsingFile(string path, SourceText actual)
    {
        string content = await File.ReadAllTextAsync(path);

        string[] expectedLines = LineEndingsHelper.Normalize(content)
            .Replace("%VERSION%", typeof(TextEmitter).Assembly.GetName().Version?.ToString())
            .Split(Environment.NewLine);

        bool areEqual = RoslynTestUtils.CompareLines(expectedLines, actual, out string errorMessage);

        if (!areEqual)
        {
            output.WriteLine(actual.ToString());
        }

        Assert.True(areEqual, errorMessage);
    }
}