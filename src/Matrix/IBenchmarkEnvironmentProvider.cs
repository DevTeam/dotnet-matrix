using System.Reflection;
using BenchmarkDotNet.Jobs;

namespace Matrix;

public interface IBenchmarkEnvironmentProvider
{
    BenchmarkEnvironment Capture(
        string benchmarkTool,
        Assembly benchmarkToolAssembly,
        string jobLabel,
        Job job);
}