using System;
using System.Threading;
using System.Threading.Tasks;
using Demo.DecoratedHandlers.Abstractions;

namespace Demo.DecoratedHandlers.Tests.Snapshots.PartialHandler;

public record Alpha;
public record Omega;

public partial class BarHandler : IRequestHandler<Alpha, Omega>
{
    public Task<Omega> HandleAsync(Alpha input, CancellationToken ct = default)
    {
        throw new NotSupportedException();
    }
}
public partial class BarHandler : IRequestHandler<Alpha, Omega>
{
    public Task<Omega> HandleAsync()
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