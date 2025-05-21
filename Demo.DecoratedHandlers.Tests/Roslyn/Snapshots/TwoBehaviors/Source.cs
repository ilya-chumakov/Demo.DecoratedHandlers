using System;
using System.Threading;
using System.Threading.Tasks;
using Demo.DecoratedHandlers.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Demo.DecoratedHandlers.Gen;

namespace Demo.DecoratedHandlers.Tests.Roslyn.Snapshots.TwoBehaviors;

public record Alpha;
public record Omega;

public class BarHandler : IRequestHandler<Alpha, Omega>
{
    public Task<Omega> HandleAsync(Alpha input, CancellationToken ct = default)
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
public class ExceptionBehavior : IPipelineBehavior<Alpha, Omega>
{
    public Task<Omega> Handle(Alpha request, RequestHandlerDelegate<Omega> next, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}