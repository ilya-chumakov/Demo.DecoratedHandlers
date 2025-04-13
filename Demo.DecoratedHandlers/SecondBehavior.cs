using Demo.DecoratedHandlers.Abstractions;
using Demo.DecoratedHandlers.Gen;
using Microsoft.Extensions.Logging;

namespace Demo.DecoratedHandlers;

[UseThisDecorator]
public class SecondBehavior<TRequest, TResponse>(ILogger<SecondBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
//where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken ct)
    {
        logger.LogInformation($"Decorating {typeof(TRequest).Name}");
        var response = await next();
        logger.LogInformation($"Decorated.");

        return response;
    }
}