/*
 * This code does not participate in source generation.
 * It provides types necessary for the static snapshot of generated code to compile.
 */


/*
 * This code does not participate in source generation.
 * It provides types necessary for the static snapshot of generated code to compile.
 */

using Demo.DecoratedHandlers.Abstractions;

namespace Demo.DecoratedHandlers.Tests.Text.Snapshots.DecoratedV1;

public record Alpha;
public record Omega;

public class FooHandler : IRequestHandler<Alpha, Omega>
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