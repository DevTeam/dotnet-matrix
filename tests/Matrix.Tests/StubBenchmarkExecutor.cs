using System.Reflection;

namespace Matrix.Tests;

internal sealed class StubBenchmarkExecutor(BenchmarkExecution execution) : IBenchmarkExecutor
{
    public Exception? Error { get; set; }
    public string? ArtifactsDirectory { get; private set; }
    public IReadOnlySet<string>? LibraryIds { get; private set; }
    public bool Smoke { get; private set; }

    public BenchmarkExecution Execute(
        Assembly moduleAssembly,
        IReadOnlySet<string> libraryIds,
        bool smoke,
        string artifactsDirectory)
    {
        ArtifactsDirectory = artifactsDirectory;
        LibraryIds = libraryIds;
        Smoke = smoke;
        File.WriteAllText(Path.Combine(artifactsDirectory, "execution.log"), "stub execution");
        if (Error is not null)
        {
            throw Error;
        }

        return execution;
    }
}
