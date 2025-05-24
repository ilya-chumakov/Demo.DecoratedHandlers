namespace Demo.DecoratedHandlers.Tests.Helpers;

public class TestDescription
{
    public File Source { get; init; }
    public ISourceDescription SourceDescription { get; init; }
    public List<File> Results { get; init; } = new();

    public class File
    {
        public string Name { get; init; }
        public string Content { get; init; }
    }
}