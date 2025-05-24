using Demo.DecoratedHandlers.Gen;
using Demo.DecoratedHandlers.Tests.Models;

namespace Demo.DecoratedHandlers.Tests.Helpers;

public static class SnapshotReader
{
    private static readonly string GeneratorAssemblyVersion =
        typeof(TextEmitter).Assembly.GetName().Version?.ToString();

    public static async Task<List<TestFile>> ReadAsync(
        string foldername,
        List<FileDescription> files)
    {
        List<TestFile> sources = new();
        foreach (var description in files)
        {
            string content = await ReadAllTextAsync(foldername, description.RelativePath);

            sources.Add(new TestFile
            {
                Name = description.GeneratedFileName,
                Content = content
            });
        }
        return sources;
    }

    public static async Task<string> ReadAllTextAsync(
        string folder, string name)
    {
        string path = Path.Combine("Snapshots", folder, name);
        string content = await File.ReadAllTextAsync(path);

        string normalized = LineEndingsHelper.Normalize(content)
            .Replace("%VERSION%", GeneratorAssemblyVersion);

        return normalized;
    }
}