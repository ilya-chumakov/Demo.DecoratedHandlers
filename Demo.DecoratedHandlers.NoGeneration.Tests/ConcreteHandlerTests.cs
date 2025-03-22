using Bodrocode.Xunit.Logs;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Demo.DecoratedHandlers.NoGeneration.Tests;

public class ConcreteHandlerTests(ITestOutputHelper output)
{
    [Fact]
    public async Task ConcreteHandler_WrapperIsCoded_HandlerIsReplaced()
    {
        var services = new ServiceCollection();
        services.AddLogging(cfg =>
        {
            cfg.AddXunit(output);
        });
        services.AddTransient<IConcreteHandler, ConcreteHandler>();
        services.AddTransient<FirstDecorator>();
        services.AddTransient<SecondDecorator>();

        // Before this line everything is registered in a natural way.
        // Now replace our handler registration with a source-generated wrapper.
        services.ReplaceHandlerWithMock();
        var provider = services.BuildServiceProvider();

        var actual = provider.GetRequiredService<IConcreteHandler>();

        await actual.HandleAsync();
        
        actual.GetType().Should().Be(typeof(MockWrapper));
    }
}