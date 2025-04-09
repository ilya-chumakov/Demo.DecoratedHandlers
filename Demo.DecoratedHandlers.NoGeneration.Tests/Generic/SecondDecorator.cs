using Microsoft.Extensions.Logging;

namespace Demo.DecoratedHandlers.NoGeneration.Tests.Generic;

public class SecondDecorator(ILogger<SecondDecorator> logger)
{
    public async Task HandleAsync(Func<Task> next)
    {
        logger.LogInformation("Hello from the decorator #2");
        await next();
        logger.LogInformation("Bye from the decorator #2");
    }
}