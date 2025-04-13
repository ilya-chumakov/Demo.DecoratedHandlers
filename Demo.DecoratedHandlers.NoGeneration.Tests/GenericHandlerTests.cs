using Bodrocode.Xunit.Logs;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Demo.DecoratedHandlers.NoGeneration.Tests;

public class GenericHandlerTests(ITestOutputHelper output)
{
    [Fact]
    public async Task ConcreteHandler_WrapperIsCoded_HandlerIsReplaced()
    {
        var services = new ServiceCollection();
        services.AddLogging(cfg =>
        {
            cfg.AddXunit(output);
        });
        services.AddTransient<IGenericHandler<BarQuery, BarResponse>, BarQueryHandler>();
        services.AddTransient(typeof(FirstBehavior<,>));
        services.AddTransient(typeof(SecondBehavior<,>));

        // Before this line everything is registered in a natural way.
        // Now replace our handler registration with a source-generated wrapper.
        services.ReplaceHandlerWithPipeline();
        var provider = services.BuildServiceProvider();

        var actual = provider.GetRequiredService<IGenericHandler<BarQuery, BarResponse>>();

        await actual.HandleAsync(new BarQuery());

        actual.GetType().Should().Be(typeof(GenericPipeline));
    }
}