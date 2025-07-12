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
        Assert.False(DummyContext.IsInvoked);

        //Act
        services.AddDecoratedHandlers([typeof(DummyContext).Assembly]);

        //Assert
        Assert.True(DummyContext.IsInvoked);
    }

    private class DummyContext : IPipelineContext
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
        Assert.False(DummyContext.IsInvoked);

        //Act
        services.AddDecoratedHandlers<DummyContext>();

        //Assert
        Assert.True(DummyContext.IsInvoked);
    }

    private class DummyContext : IPipelineContext
    {
        public static bool IsInvoked { get; set; }

        public void Apply(IServiceCollection services)
        {
            IsInvoked = true;
        }
    }
}

