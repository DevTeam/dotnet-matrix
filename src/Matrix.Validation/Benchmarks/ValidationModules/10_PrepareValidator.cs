using System.Diagnostics.CodeAnalysis;
// ReSharper disable CheckNamespace
namespace Matrix.Validation.Benchmarks;

public partial class PrepareValidator
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ValidationModules)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public BasicInputValidator ValidationModules() => new();
}
