using System;
using System.Threading;
using System.Threading.Tasks;
using Demo.DecoratedHandlers.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Demo.DecoratedHandlers.Gen;

namespace MyNamespace;

public record Alpha;
public record Omega;

public interface IGenericHandler<TInput, TOutput> { }

[Demo.DecoratedHandlers.Abstractions.DecorateThisHandler]
public class Bar : IGenericHandler<Alpha, Omega> { 
    public Task<Omega> HandleAsync(Alpha input, CancellationToken ct = default)
    {
        return Task.FromResult(new Omega());
    }
}