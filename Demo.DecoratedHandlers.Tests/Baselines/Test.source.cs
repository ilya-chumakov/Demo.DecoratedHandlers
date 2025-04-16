using Demo.DecoratedHandlers.Abstractions;
// ReSharper disable CheckNamespace

namespace FooNamespace;

public record Alpha;
public record Omega;

public class FooHandler : IGenericHandler<Alpha, Omega>
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