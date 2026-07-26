// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class ChildContainer
{
    [Benchmark(Baseline = true)]
    [LibraryBenchmark(LibraryCatalog.HandCoded, true)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public BenchmarkRoots<ChildRoot, ChildRoot> HandCoded()
    {
        return new(
            new ChildRoot(new ParentValue()),
            new ChildRoot(new ChildValue()));
    }
}
