using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Demo.DecoratedHandlers.NoGeneration.Tests;

public class MockWrapper(IServiceProvider provider) : IConcreteHandler
{
    public Task HandleAsync()
    {
        var decorator1 = provider.GetRequiredService<FirstDecorator>();
        var decorator2 = provider.GetRequiredService<SecondDecorator>();
        var handler = provider.GetRequiredService<ConcreteHandler>();

        var hf = () => handler.HandleAsync();
        var df1 = () => decorator1.HandleAsync(hf);
        var df2 = () => decorator2.HandleAsync(df1);

        return df2();
    }
}

public static class ServiceCollectionExtensions
{
    public static void ReplaceHandlerWithMock(this IServiceCollection services)
    {
        services.RemoveAll<IConcreteHandler>();
        services.AddTransient<IConcreteHandler, MockWrapper>();
        services.AddTransient<ConcreteHandler>();
        
    }
}