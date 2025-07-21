using Demo.DecoratedHandlers.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.DecoratedHandlers.Tests.Abstractions;

public class AddDecoratedHandlersExtension_ReflectionScan_Tests
{
    // ReSharper disable once InconsistentNaming
    private readonly ServiceCollection services = new();

    [Fact]
    public void AddDecoratedHandlers_Default_OK()
    {
        //Arrange
        Assert.False(DummyRegistry.IsInvoked);

        //Act
        services.AddDecoratedHandlers(options =>
        {
            options.ScanAssemblies = [typeof(DummyRegistry).Assembly];
        });

        //Assert
        Assert.True(DummyRegistry.IsInvoked);
    }

    private class DummyRegistry : IPipelineRegistry
    {
        public static bool IsInvoked { get; set; }

        public void Apply(IServiceCollection services)
        {
            IsInvoked = true;
        }
    }
}

public class AddDecoratedHandlersExtension_GenericParam_Tests
{
    // ReSharper disable once InconsistentNaming
    private readonly ServiceCollection services = new();

    [Fact]
    public void AddDecoratedHandlers_Default_OK()
    {
        //Arrange
        Assert.False(DummyRegistry.IsInvoked);

        //Act
        services.AddDecoratedHandlers<DummyRegistry>();

        //Assert
        Assert.True(DummyRegistry.IsInvoked);
    }

    private class DummyRegistry : IPipelineRegistry
    {
        public static bool IsInvoked { get; set; }

        public void Apply(IServiceCollection services)
        {
            IsInvoked = true;
        }
    }
}

