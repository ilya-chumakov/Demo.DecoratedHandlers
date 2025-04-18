﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Demo.DecoratedHandlers.NoGeneration.Tests;

public class GenericPipeline(IServiceProvider provider) : IGenericHandler<BarQuery, BarResponse>
{
    public Task<BarResponse> HandleAsync(BarQuery command, CancellationToken ct = default)
    {
        var b1 = provider.GetRequiredService<FirstBehavior<BarQuery, BarResponse>>();
        var b2 = provider.GetRequiredService<SecondBehavior<BarQuery, BarResponse>>();
        var handler = provider.GetRequiredService<BarQueryHandler>();

        RequestHandlerDelegate<BarResponse> original = () => handler.HandleAsync(command, ct);

        RequestHandlerDelegate<BarResponse> df1 = () => b1.Handle(command, original, ct);

        RequestHandlerDelegate<BarResponse> df2 = () => b2.Handle(command, df1, ct);

        return df2();
    }
}

public static class ServiceCollectionExtensions
{
    public static void ReplaceHandlerWithPipeline(this IServiceCollection services)
    {
        services.RemoveAll<IGenericHandler<BarQuery, BarResponse>>();
        services.AddTransient<IGenericHandler<BarQuery, BarResponse>, GenericPipeline>();
        services.AddTransient<BarQueryHandler>();
    }
}