using Microsoft.Extensions.Logging;

namespace Demo.DecoratedHandlers.NoGeneration.Tests.Generic;

public class FooCommandHandler(ILogger<FooCommandHandler> logger) 
    : IGenericHandler<FooCommand, FooCommandResponse>
{
    public Task<FooCommandResponse> HandleAsync(FooCommand input, CancellationToken ct = default)
    {
        logger.LogInformation("FooHandler is called!");
        return Task.FromResult(new FooCommandResponse(42.ToString()));
    }
}