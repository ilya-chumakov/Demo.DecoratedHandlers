using Microsoft.Extensions.DependencyInjection;

namespace Demo.DecoratedHandlers.Abstractions;

public interface IPipelineRegistry
{
    public void Apply(IServiceCollection services);
}