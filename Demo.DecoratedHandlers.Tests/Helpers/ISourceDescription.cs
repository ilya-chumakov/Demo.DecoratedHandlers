using Demo.DecoratedHandlers.Gen;

namespace Demo.DecoratedHandlers.Tests.Helpers;

public interface ISourceDescription
{
    public string FolderName { get; init; }
    public List<HandlerDescription> Handlers { get; init; }
    public List<BehaviorDescription> Behaviors { get; init; }
}