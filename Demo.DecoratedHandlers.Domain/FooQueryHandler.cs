using Demo.DecoratedHandlers.Abstractions;
using Microsoft.Extensions.Logging;

namespace Demo.DecoratedHandlers.Domain;

public class FooQueryHandler(ILogger<FooQueryHandler> logger) 
    : IGenericHandler<FooQuery, FooResponse>
{
    public Task<FooResponse> HandleAsync(FooQuery input, CancellationToken ct = default)
    {
        logger.LogInformation($"{nameof(FooQueryHandler)} is called!");
        return Task.FromResult(new FooResponse());
    }
}