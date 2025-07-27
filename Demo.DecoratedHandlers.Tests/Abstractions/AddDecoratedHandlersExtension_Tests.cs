using Demo.DecoratedHandlers.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.DecoratedHandlers.Tests.Abstractions;

public class AddDecoratedHandlersExtension_AssemblyScan_Tests
{
    // ReSharper disable once InconsistentNaming
    private readonly ServiceCollection services = new();

    [Fact]
    public void AddDecoratedHandlers_AssemblyScan_OK()
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
    
    [Fact]
    public void AddDecoratedHandlers_AssemblyScanDouble_NoNewRegistrations()
    {
        //Arrange
        services.AddDecoratedHandlers(options =>
        {
            options.ScanAssemblies = [typeof(DummyRegistry).Assembly];
        });
        int expected = services.Count;

        //Act
        services.AddDecoratedHandlers(options =>
        {
            options.ScanAssemblies = [typeof(DummyRegistry).Assembly];
        });

        //Assert
        Assert.Equal(expected, services.Count);
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
    public void AddDecoratedHandlers_GenericParam_OK()
    {
        //Arrange
        Assert.False(DummyRegistry.IsInvoked);

        //Act
        services.AddDecoratedHandlers<DummyRegistry>();

        //Assert
        Assert.True(DummyRegistry.IsInvoked);
    }

    [Fact]
    public void AddDecoratedHandlers_GenericParamDouble_NoNewRegistrations()
    {
        //Arrange
        services.AddDecoratedHandlers<DummyRegistry>();
        int expected = services.Count;

        //Act
        services.AddDecoratedHandlers<DummyRegistry>();

        //Assert
        Assert.Equal(expected, services.Count);
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

