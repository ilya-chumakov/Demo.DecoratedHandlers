using System.Reflection;
using Demo.DecoratedHandlers.Gen;
using FluentAssertions;
using Xunit.Abstractions;

namespace Demo.DecoratedHandlers.Tests;

public class ConcreteHandlerTests(ITestOutputHelper output)
{
    [Fact]
    public void Wrapper_AvailableViaReflection_OK()
    {
        var obj = new ConcreteHandler(null);
        var assembly = Assembly.GetAssembly(typeof(ConcreteHandler));

        var type = assembly
            .GetType("Demo.DecoratedHandlers.ConcreteHandlerPipeline", true, true)
            .Should().NotBeNull();

        var x = new ConcreteHandlerPipeline(null);
    }

    [Fact]
    public void Wrapper_AvailableInCompileTime_OK()
    {
        var x = new ConcreteHandlerPipeline(null);
    }

    [Fact]
    public void Debug()
    {
        output.WriteLine("Stage #1");
        foreach (string name in Registrations.Debug())
        {
            output.WriteLine(name);
        }

        output.WriteLine("Stage #2");
        output.WriteLine(typeof(ConcreteHandler).Assembly.FullName);
        var wr = new WeakReference(new ConcreteHandler(null));

        output.WriteLine("Stage #3");
        foreach (string name in Registrations.Debug())
        {
            output.WriteLine(name);
        }
    }
}