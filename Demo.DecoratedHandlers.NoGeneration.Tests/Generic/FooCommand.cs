namespace Demo.DecoratedHandlers.NoGeneration.Tests.Generic;

public class FooCommand
{
    public string Id { get; private set; }
}

public class FooCommandResponse
{

    public FooCommandResponse(string id)
    {
        Id = id;
    }

    public string Id { get; private set; }
}