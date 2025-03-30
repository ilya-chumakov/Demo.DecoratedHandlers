using Demo.DecoratedHandlers.Gen;
using Microsoft.Extensions.Logging;

namespace Demo.DecoratedHandlers;

[DecorateThisHandler]
public class FooCommandHandler(ILogger<FooCommandHandler> logger) : IGenericHandler<FooQuery>
{
    public Task HandleAsync(FooQuery query)
    {
        logger.LogInformation($"{nameof(FooCommandHandler)} is called!");
        return Task.CompletedTask;
    }
}

public class FooQuery
{
}

// not used
public class FooResponse
{
}