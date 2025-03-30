using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Demo.DecoratedHandlers.NoGeneration.Tests.Generic;

public class GenericSequence(IServiceProvider provider) : IGenericHandler<FooCommand>
{
    public Task HandleAsync(FooCommand command)
    {
        var decorator1 = provider.GetRequiredService<FirstDecorator>();
        var decorator2 = provider.GetRequiredService<SecondDecorator>();
        var handler = provider.GetRequiredService<FooCommandHandler>();

        var hf = () => handler.HandleAsync(command);
        var df1 = () => decorator1.HandleAsync(hf);
        var df2 = () => decorator2.HandleAsync(df1);

        return df2();
    }
}

public static class ServiceCollectionExtensions
{
    public static void ReplaceHandlerWithSequence(this IServiceCollection services)
    {
        services.RemoveAll<IGenericHandler<FooCommand>>();
        services.AddTransient<IGenericHandler<FooCommand>, GenericSequence>();
        services.AddTransient<FooCommandHandler>();
    }
}