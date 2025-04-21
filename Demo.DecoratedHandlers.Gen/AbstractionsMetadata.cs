namespace Demo.DecoratedHandlers.Gen;

public class AbstractionsMetadata
{
    public static AbstractionsMetadata Instance { get; internal set; } = new();

    public const string RequestInterfaceSymbolName = "IGenericHandler";
    public const string BehaviorInterfaceSymbolName = "IPipelineBehavior";
    public const string AssemblySymbolName = "Demo.DecoratedHandlers.Abstractions";
}