// ReSharper disable CheckNamespace
using System;
using System.Threading;
using System.Threading.Tasks;
using Demo.DecoratedHandlers.Abstractions;

namespace Demo.DecoratedHandlers.Tests.Snapshots.SameName.RequestNamespace
{
    public record Foo;
}

namespace Demo.DecoratedHandlers.Tests.Snapshots.SameName.ResponseNamespace
{
    public record Foo;
}

namespace Demo.DecoratedHandlers.Tests.Snapshots.SameName.HandlerNamespace
{

    public class Foo : IRequestHandler<RequestNamespace.Foo, ResponseNamespace.Foo>
    {
        public Task<ResponseNamespace.Foo> HandleAsync(RequestNamespace.Foo input, CancellationToken ct = default)
        {
            throw new NotSupportedException();
        }
    }
}

namespace Demo.DecoratedHandlers.Tests.Snapshots.SameName.BehaviorNamespace
{
    public class Foo<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        public Task<TResponse> Handle(
            TRequest request, 
            RequestHandlerDelegate<TResponse> next, 
            CancellationToken ct = default)
        {
            throw new NotSupportedException();
        }
    }
}