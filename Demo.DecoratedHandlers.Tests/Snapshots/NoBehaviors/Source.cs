using System;
using System.Threading;
using System.Threading.Tasks;
using Demo.DecoratedHandlers.Abstractions;

namespace Demo.DecoratedHandlers.Tests.Snapshots.NoBehaviors;

public record Alpha;
public record Omega;

public class BarHandler : IRequestHandler<Alpha, Omega>
{
    public Task<Omega> HandleAsync(Alpha input, CancellationToken ct = default)
    {
        return Task.FromResult(new Omega());
    }
}