using Microsoft.Extensions.Logging;

namespace Demo.DecoratedHandlers.NoGeneration.Tests;

public class BarQueryHandler(ILogger<BarQueryHandler> logger) 
    : IRequestHandler<BarQuery, BarResponse>
{
    public Task<BarResponse> HandleAsync(BarQuery input, CancellationToken ct = default)
    {
        logger.LogInformation($"{nameof(BarQueryHandler)} is called!");
        return Task.FromResult(new BarResponse(42.ToString()));
    }
}