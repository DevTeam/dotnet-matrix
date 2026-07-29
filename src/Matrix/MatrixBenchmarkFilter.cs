using BenchmarkDotNet.Filters;
using BenchmarkDotNet.Running;
using System.Reflection;

namespace Matrix;

internal sealed class MatrixBenchmarkFilter(IReadOnlySet<string> libraryIds) : IFilter
{
    public bool Predicate(BenchmarkCase benchmarkCase)
    {
        var metadata = benchmarkCase.Descriptor.WorkloadMethod
            .GetCustomAttribute<LibraryBenchmarkAttribute>();
        return metadata is not null && libraryIds.Contains(metadata.LibraryId);
    }
}
