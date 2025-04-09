using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Demo.DecoratedHandlers.NoGeneration.Tests.Generic;

public class GenericPipeline(IServiceProvider provider) : IGenericHandler<FooCommand, FooCommandResponse>
{
    public Task<FooCommandResponse> HandleAsync(FooCommand command, CancellationToken ct = default)
    {
        var b1 = provider.GetRequiredService<FirstBehavior<FooCommand, FooCommandResponse>>();
        var b2 = provider.GetRequiredService<SecondBehavior<FooCommand, FooCommandResponse>>();
        var handler = provider.GetRequiredService<FooCommandHandler>();

        RequestHandlerDelegate<FooCommandResponse> original = () => handler.HandleAsync(command, ct);

        RequestHandlerDelegate<FooCommandResponse> df1 = () => b1.Handle(command, original, ct);

        RequestHandlerDelegate<FooCommandResponse> df2 = () => b2.Handle(command, df1, ct);

        return df2();
    }
}

public static class ServiceCollectionExtensions
{
    public static void ReplaceHandlerWithPipeline(this IServiceCollection services)
    {
        services.RemoveAll<IGenericHandler<FooCommand, FooCommandResponse>>();
        services.AddTransient<IGenericHandler<FooCommand, FooCommandResponse>, GenericPipeline>();
        services.AddTransient<FooCommandHandler>();
    }
}