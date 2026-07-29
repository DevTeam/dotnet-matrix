using System.Reflection;

namespace Matrix;

public interface IBenchmarkEnvironmentProvider
{
    BenchmarkEnvironment Capture(
        string benchmarkTool,
        Assembly benchmarkToolAssembly,
        string job);
}