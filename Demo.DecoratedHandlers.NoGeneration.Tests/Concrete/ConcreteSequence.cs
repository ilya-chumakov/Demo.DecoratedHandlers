using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Demo.DecoratedHandlers.NoGeneration.Tests.Concrete;

public class ConcreteSequence(IServiceProvider provider) : IConcreteHandler
{
    public Task HandleAsync()
    {
        var decorator1 = provider.GetRequiredService<FirstDecorator>();
        var decorator2 = provider.GetRequiredService<SecondDecorator>();
        var handler = provider.GetRequiredService<GenericHandler>();

        var hf = () => handler.HandleAsync();
        var df1 = () => decorator1.HandleAsync(hf);
        var df2 = () => decorator2.HandleAsync(df1);

        return df2();
    }
}

public static class ServiceCollectionExtensions
{
    public static void ReplaceHandlerWithSequence(this IServiceCollection services)
    {
        services.RemoveAll<IConcreteHandler>();
        services.AddTransient<IConcreteHandler, ConcreteSequence>();
        services.AddTransient<GenericHandler>();
    }
}