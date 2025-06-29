﻿namespace Demo.DecoratedHandlers.Abstractions;

public interface IRequestHandler<in TInput, TOutput>
{
    Task<TOutput> HandleAsync(TInput input, CancellationToken ct = default);
}