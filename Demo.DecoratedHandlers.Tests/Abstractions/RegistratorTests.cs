using Demo.DecoratedHandlers.Abstractions;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Demo.DecoratedHandlers.Tests.Abstractions;

public class RegistratorTests
{
    // ReSharper disable once InconsistentNaming
    private readonly ServiceCollection services = new();

    [Fact]
    public void ReplaceWithPipeline_Default_OK()
    {
        //Arrange
        services.AddTransient<IRequestHandler<FooInput, FooOutput>, FooHandler>();

        //Act
        services.ReplaceWithPipeline<IRequestHandler<FooInput, FooOutput>, FooHandler, FooHandlerPipeline>();

        //Assert
        For<IRequestHandler<FooInput, FooOutput>, FooHandler>().Should().BeEmpty();
        For<FooHandler, FooHandler>().Should().ContainSingle();
        
        For<IRequestHandler<FooInput, FooOutput>, FooHandlerPipeline>().Should().ContainSingle();
        For<FooHandlerPipeline, FooHandlerPipeline>().Should().BeEmpty();

        services.Where(d => 
                d.ServiceType == typeof(IRequestHandler<,>) || 
                d.ImplementationType == typeof(IRequestHandler<,>))
            .Should().BeEmpty();
    }

    [Theory]
    [InlineData(ServiceLifetime.Singleton)]
    [InlineData(ServiceLifetime.Scoped)]
    [InlineData(ServiceLifetime.Transient)]
    public void ReplaceWithPipeline_CustomLifetime_RespectsLifetime(ServiceLifetime lifetime)
    {
        //Arrange
        services.Add(new ServiceDescriptor(
            typeof(IRequestHandler<FooInput, FooOutput>),
            typeof(FooHandler),
            lifetime));

        //Act
        services.ReplaceWithPipeline<IRequestHandler<FooInput, FooOutput>, FooHandler, FooHandlerPipeline>();

        //Assert
        var descriptor = For<IRequestHandler<FooInput, FooOutput>, FooHandlerPipeline>().Single();

        descriptor.Lifetime.Should().Be(lifetime);
    }

    [Fact]
    public void ReplaceWithPipeline_NoRegistration_NoPipeline()
    {
        //Act
        services.ReplaceWithPipeline<IRequestHandler<FooInput, FooOutput>, FooHandler, FooHandlerPipeline>();

        //Assert
        For<IRequestHandler<FooInput, FooOutput>, FooHandler>().Should().BeEmpty();
        For<FooHandler, FooHandler>().Should().BeEmpty();
        
        For<IRequestHandler<FooInput, FooOutput>, FooHandlerPipeline>().Should().BeEmpty();
        For<FooHandlerPipeline, FooHandlerPipeline>().Should().BeEmpty();
    }

    private IEnumerable<ServiceDescriptor> For<TKey, TImpl>()
    {
        return services.Where(d => d.ServiceType == typeof(TKey) && d.ImplementationType == typeof(TImpl));
    }

    private record FooInput;

    private record FooOutput(string Name);

    private class FooHandler : IRequestHandler<FooInput, FooOutput>
    {
        public Task<FooOutput> HandleAsync(FooInput input, CancellationToken ct = default) => Task.FromResult(new FooOutput("fooName"));
    }

    private class FooHandlerPipeline : IRequestHandler<FooInput, FooOutput>
    {
        public Task<FooOutput> HandleAsync(FooInput input, CancellationToken ct = default) => Task.FromResult(new FooOutput("fooName"));
    }
}