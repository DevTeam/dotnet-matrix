using System.Reflection;

namespace Matrix;

public interface IBenchmarkExecutor
{
    BenchmarkExecution Execute(
        Assembly moduleAssembly,
        IReadOnlySet<string> libraryIds,
        bool smoke,
        string artifactsDirectory);
}
