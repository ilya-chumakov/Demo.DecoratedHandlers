using Demo.DecoratedHandlers.Gen;
using Microsoft.Extensions.Logging;

namespace Demo.DecoratedHandlers;

[DecorateThisHandler]
public class ConcreteHandler(ILogger<ConcreteHandler> logger) : IConcreteHandler
{
    public Task HandleAsync()
    {
        logger.LogInformation($"{nameof(ConcreteHandler)} is called!");
        return Task.CompletedTask;
    }
}

// not used
public class FooQuery
{
}

public class FooResponse
{
}