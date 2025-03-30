using Microsoft.Extensions.Logging;

namespace Demo.DecoratedHandlers.NoGeneration.Tests.Concrete;

public interface IConcreteHandler
{
    Task HandleAsync();
}

public class GenericHandler(ILogger<GenericHandler> logger) : IConcreteHandler
{
    public Task HandleAsync()
    {
        logger.LogInformation("FooHandler is called!");
        return Task.CompletedTask;
    }
}

public class FirstDecorator(ILogger<FirstDecorator> logger)
{
    public async Task HandleAsync(Func<Task> next)
    {
        logger.LogInformation("Hello from the decorator #1");
        await next();
        logger.LogInformation("Bye from the decorator #1");
    }
}

public class SecondDecorator(ILogger<SecondDecorator> logger)
{
    public async Task HandleAsync(Func<Task> next)
    {
        logger.LogInformation("Hello from the decorator #2");
        await next();
        logger.LogInformation("Bye from the decorator #2");
    }
}