using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Demo.DecoratedHandlers.WebApiRoot;

public class DelayedLogHostedService(ILogger<DelayedLogHostedService> logger) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        DecoratedHandlers.Abstractions.AddDecoratedHandlersExtension.Log.Apply(logger);

        return Task.CompletedTask;
    }
    
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}