namespace Demo.DecoratedHandlers.Abstractions;

public interface IGenericHandler<TInput, TOutput>
{
    Task<TOutput> HandleAsync(TInput input, CancellationToken ct = default);
}