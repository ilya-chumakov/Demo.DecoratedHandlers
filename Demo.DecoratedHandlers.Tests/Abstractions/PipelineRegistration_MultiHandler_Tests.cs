using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Demo.DecoratedHandlers.Abstractions;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.DecoratedHandlers.Tests.Abstractions;

public class PipelineRegistration_MultiHandler_Tests
{
    // ReSharper disable once InconsistentNaming
    private readonly ServiceCollection services = new();

    [Fact(Skip = "This is an assumption on how to register a multi handler. Generation for this case is not implemented yet")]
    public void ReplaceWithPipeline_Default_OK()
    {
        //Arrange
        services.AddTransient<IRequestHandler<FooInput, FooOutput>, MultiHandler>();
        services.AddTransient<IRequestHandler<BarInput, BarOutput>, MultiHandler>();

        //Act
        services.ReplaceWithPipeline<IRequestHandler<FooInput, FooOutput>, MultiHandler, FooHandlerPipeline>();
        services.ReplaceWithPipeline<IRequestHandler<BarInput, BarOutput>, MultiHandler, BarHandlerPipeline>();

        //Assert
        //...Foo
        For<IRequestHandler<FooInput, FooOutput>, MultiHandler>().Should().BeEmpty();
        For<MultiHandler, MultiHandler>().Should().ContainSingle();
        
        For<IRequestHandler<FooInput, FooOutput>, FooHandlerPipeline>().Should().ContainSingle();
        For<FooHandlerPipeline, FooHandlerPipeline>().Should().BeEmpty();
        
        //...Bar
        For<IRequestHandler<BarInput, BarOutput>, MultiHandler>().Should().BeEmpty();
        For<MultiHandler, MultiHandler>().Should().ContainSingle();
        
        For<IRequestHandler<BarInput, BarOutput>, BarHandlerPipeline>().Should().ContainSingle();
        For<BarHandlerPipeline, BarHandlerPipeline>().Should().BeEmpty();

        //...Generic
        services.Where(d => 
                d.ServiceType == typeof(IRequestHandler<,>) || 
                d.ImplementationType == typeof(IRequestHandler<,>))
            .Should().BeEmpty();
    }

    private IEnumerable<ServiceDescriptor> For<TKey, TImpl>()
    {
        return services.Where(d => d.ServiceType == typeof(TKey) && d.ImplementationType == typeof(TImpl));
    }

    private record FooInput;
    private record FooOutput(string Name);

    private record BarInput;
    private record BarOutput(int Id);

    private class MultiHandler 
        : IRequestHandler<FooInput, FooOutput>
        , IRequestHandler<BarInput, BarOutput>
    {
        public Task<FooOutput> HandleAsync(FooInput input, CancellationToken ct = default)
        {
            return Task.FromResult(new FooOutput("fooName"));
        }

        public Task<BarOutput> HandleAsync(BarInput input, CancellationToken ct = default)
        {
            return Task.FromResult(new BarOutput(42));
        }
    }

    private class FooHandlerPipeline : IRequestHandler<FooInput, FooOutput>
    {
        public Task<FooOutput> HandleAsync(FooInput input, CancellationToken ct = default)
        {
            return Task.FromResult(new FooOutput("fooName"));
        }
    }

    private class BarHandlerPipeline : IRequestHandler<BarInput, BarOutput>
    {
        public Task<BarOutput> HandleAsync(BarInput input, CancellationToken ct = default)
        {
            return Task.FromResult(new BarOutput(42));
        }
    }
}