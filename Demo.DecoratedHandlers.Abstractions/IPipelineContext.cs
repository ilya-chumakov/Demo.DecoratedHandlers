using Microsoft.Extensions.DependencyInjection;

namespace Demo.DecoratedHandlers.Abstractions;

public interface IPipelineContext
{
    public void Apply(IServiceCollection services);
}