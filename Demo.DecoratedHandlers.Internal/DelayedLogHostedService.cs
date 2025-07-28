using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Demo.DecoratedHandlers.Gen;

internal class DelayedLogHostedService(ILogger<DelayedLogHostedService> logger) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        AddDecoratedHandlersExtension.Log.Apply(logger);

        return Task.CompletedTask;
    }
    
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}