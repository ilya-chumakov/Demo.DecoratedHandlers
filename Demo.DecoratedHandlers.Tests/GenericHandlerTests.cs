using System.Reflection;
using Demo.DecoratedHandlers.Gen;
using FluentAssertions;
using Xunit.Abstractions;

namespace Demo.DecoratedHandlers.Tests;

public class GenericHandlerTests(ITestOutputHelper output)
{
    [Fact]
    public void Wrapper_AvailableViaReflection_OK()
    {
        var obj = new FooCommandHandler(null);
        var assembly = Assembly.GetAssembly(typeof(FooCommandHandler));

        var type = assembly
            .GetType("Demo.DecoratedHandlers.FooCommandHandlerPipeline", true, true)
            .Should().NotBeNull();

        var x = new FooCommandHandlerPipeline(null);
    }

    [Fact]
    public void Wrapper_AvailableInCompileTime_OK()
    {
        IGenericHandler<FooQuery> x = new FooCommandHandlerPipeline(null);
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
        output.WriteLine(typeof(FooCommandHandler).Assembly.FullName);
        var wr = new WeakReference(new FooCommandHandler(null));

        output.WriteLine("Stage #3");
        foreach (string name in Registrations.Debug())
        {
            output.WriteLine(name);
        }
    }
}