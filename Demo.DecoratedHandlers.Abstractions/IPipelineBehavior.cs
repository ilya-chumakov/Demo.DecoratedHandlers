﻿namespace Demo.DecoratedHandlers.Abstractions;

public interface IPipelineBehavior<in TRequest, TResponse> //where TRequest : notnull
{
    Task<TResponse> Handle(TRequest request, 
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct);
}