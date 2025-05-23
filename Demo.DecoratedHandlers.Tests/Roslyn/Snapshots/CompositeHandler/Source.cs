using System;
using System.Threading;
using System.Threading.Tasks;
using Demo.DecoratedHandlers.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Demo.DecoratedHandlers.Gen;

namespace Demo.DecoratedHandlers.Tests.Roslyn.Snapshots.CompositeHandler;

public record Alpha;
public record Beta;
public record Omega;

public class BarHandler : IRequestHandler<Alpha, Omega>, IRequestHandler<Beta, Omega>
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
public class LogBehavior : IPipelineBehavior<Alpha, Omega>
{
    public Task<Omega> Handle(Alpha request, RequestHandlerDelegate<Omega> next, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}