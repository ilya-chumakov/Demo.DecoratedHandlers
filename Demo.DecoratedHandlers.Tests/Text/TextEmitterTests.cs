using Demo.DecoratedHandlers.Gen;
using FooNamespace;
using Microsoft.CodeAnalysis.Text;
using SourceGenerators.Tests;
using Xunit.Abstractions;

namespace Demo.DecoratedHandlers.Tests.Text;

public class TextEmitterTests(ITestOutputHelper output)
{
    [Fact]
    public async Task Test_Baseline()
    {
        SourceText actual = TextEmitter.CreatePipelineSource(
            new HandlerDescription(
                HandlerTypeName: nameof(FooHandler),
                InputTypeName: nameof(Alpha),
                OutputTypeName: nameof(Omega),
                OutputNamespace: nameof(FooNamespace)),
            [
                new BehaviorDescription(nameof(LogBehavior)), 
                new BehaviorDescription(nameof(ExceptionBehavior))
            ]
        );

        await VerifyAgainstBaselineUsingFile("Test.generated.cs", actual);
    }

    private async Task VerifyAgainstBaselineUsingFile(string filename, SourceText actual)
    {
        string content = await File.ReadAllTextAsync(Path.Combine("Baselines", filename));

        string baseline = LineEndingsHelper.Normalize(content);
        string[] expectedLines = baseline
            //.Replace("%VERSION%", typeof(TextEmitter).Assembly.GetName().Version?.ToString())
            .Split(Environment.NewLine);

        bool isOk = RoslynTestUtils.CompareLines(expectedLines, actual, out string errorMessage);

        if (!isOk)
        {
            output.WriteLine(actual.ToString());
        }

        Assert.True(isOk, errorMessage);
    }
}