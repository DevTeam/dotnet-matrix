using BenchmarkDotNet.Filters;
using BenchmarkDotNet.Running;
using System.Reflection;

namespace Matrix.DependencyInjection.Benchmarking;

internal sealed class LibraryFilter(IReadOnlySet<string> libraryIds) : IFilter
{
    public bool Predicate(BenchmarkCase benchmarkCase)
    {
        var method = benchmarkCase.Descriptor.WorkloadMethod;
        var metadata = method.GetCustomAttribute<LibraryBenchmarkAttribute>();
        if (metadata is null)
        {
            return false;
        }

        if (!metadata.Baseline)
        {
            return libraryIds.Contains(metadata.LibraryId);
        }

        return method.DeclaringType!
            .GetMethods(BindingFlags.Instance | BindingFlags.Public)
            .Select(candidate => candidate.GetCustomAttribute<LibraryBenchmarkAttribute>())
            .Any(candidate =>
                candidate is { Baseline: false }
                && libraryIds.Contains(candidate.LibraryId));
    }
}
