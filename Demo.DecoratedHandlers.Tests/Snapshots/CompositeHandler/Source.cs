using System;
using System.Threading;
using System.Threading.Tasks;
using Demo.DecoratedHandlers.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Demo.DecoratedHandlers.Gen;

namespace Demo.DecoratedHandlers.Tests.Snapshots.CompositeHandler;

public record Alpha;
public record Beta;
public record Omega;

public class BarHandler : 
    IRequestHandler<Alpha, Omega>, 
    IRequestHandler<Beta, Omega>
{
    public Task<Omega> HandleAsync(Alpha input, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<Omega> HandleAsync(Beta input, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}
public class LogBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    public Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}