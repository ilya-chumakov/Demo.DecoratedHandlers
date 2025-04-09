using System;

namespace Demo.DecoratedHandlers.Gen;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class DecorateThisHandler : Attribute
{
}

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class UseThisDecorator : Attribute
{
}

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public class RegisterThis : Attribute
{
}