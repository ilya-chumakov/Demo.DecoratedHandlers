namespace Demo.DecoratedHandlers.NoGeneration.Tests.Generic;

public interface IGenericHandler<in TInput, TOutput>
{
    Task<TOutput> HandleAsync(TInput input, CancellationToken ct = default);
}