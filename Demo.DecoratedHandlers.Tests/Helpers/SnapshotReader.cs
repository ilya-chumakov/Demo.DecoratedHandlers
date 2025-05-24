using Demo.DecoratedHandlers.Gen;

namespace Demo.DecoratedHandlers.Tests.Helpers;

public static class SnapshotReader
{
    private static readonly string GeneratorAssemblyVersion =
        typeof(TextEmitter).Assembly.GetName().Version?.ToString();

    private static async Task<string> ReadSnapshotAsync(string snapshot, string file)
    {
        string path = Path.Combine("Snapshots\\" + snapshot, file);

        return await File.ReadAllTextAsync(path);
    }

    public static async Task<TestDescription.File> ReadSourceAsync(string snapshotCodename,
        string snapshotFilename = "Source.cs")
    {
        return new TestDescription.File
        {
            Name = snapshotFilename,
            Content = await ReadSnapshotAsync(snapshotCodename, snapshotFilename)
        };
    }

    public static async Task<TestDescription.File> ReadExpectedAsync(
        string snapshotCodename,
        string snapshotFilename = "Generated.cs",
        string generatedFilename = "BarHandler_Pipeline.g.cs")
    {
        string content = await ReadSnapshotAsync(snapshotCodename, snapshotFilename);


        string normalized = LineEndingsHelper.Normalize(content)
            .Replace("%VERSION%", GeneratorAssemblyVersion);

        return new TestDescription.File
        {
            Name = generatedFilename,
            Content = normalized
        };
    }
}