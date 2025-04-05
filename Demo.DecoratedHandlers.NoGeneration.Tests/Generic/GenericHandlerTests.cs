using Bodrocode.Xunit.Logs;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Demo.DecoratedHandlers.NoGeneration.Tests.Generic;

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
        services.AddTransient<IGenericHandler<FooCommand>, FooCommandHandler>();
        services.AddTransient<FirstDecorator>();
        services.AddTransient<SecondDecorator>();

        // Before this line everything is registered in a natural way.
        // Now replace our handler registration with a source-generated wrapper.
        services.ReplaceHandlerWithPipeline();
        var provider = services.BuildServiceProvider();

        var actual = provider.GetRequiredService<IGenericHandler<FooCommand>>();

        await actual.HandleAsync(new FooCommand());

        actual.GetType().Should().Be(typeof(GenericPipeline));
    }
}