using System;
using System.Threading;
using System.Threading.Tasks;
using Demo.DecoratedHandlers.Abstractions;

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
        throw new NotSupportedException();
    }

    public Task<Omega> HandleAsync(Beta input, CancellationToken ct = default)
    {
        throw new NotSupportedException();
    }
}
public class LogBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    public Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken ct = default)
    {
        throw new NotSupportedException();
    }
}