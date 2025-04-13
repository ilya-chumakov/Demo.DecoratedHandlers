namespace Demo.DecoratedHandlers.NoGeneration.Tests;

public interface IGenericHandler<in TInput, TOutput>
{
    Task<TOutput> HandleAsync(TInput input, CancellationToken ct = default);
}