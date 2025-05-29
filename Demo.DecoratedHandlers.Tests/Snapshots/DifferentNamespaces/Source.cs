// ReSharper disable CheckNamespace
using Demo.DecoratedHandlers.Abstractions;
using Demo.DecoratedHandlers.Tests.Snapshots.DifferentNamespaces.RequestNamespace;
using Demo.DecoratedHandlers.Tests.Snapshots.DifferentNamespaces.ResponseNamespace;

namespace Demo.DecoratedHandlers.Tests.Snapshots.DifferentNamespaces.RequestNamespace
{
    public record Alpha;
}

namespace Demo.DecoratedHandlers.Tests.Snapshots.DifferentNamespaces.ResponseNamespace
{
    public record Omega;
}

namespace Demo.DecoratedHandlers.Tests.Snapshots.DifferentNamespaces.HandlerNamespace
{

    public class BarHandler : IRequestHandler<Alpha, Omega>
    {
        public Task<Omega> HandleAsync(Alpha input, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}

namespace Demo.DecoratedHandlers.Tests.Snapshots.DifferentNamespaces.BehaviorNamespace
{
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
}