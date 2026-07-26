// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Transient
{
    [Benchmark(Baseline = true)]
    [LibraryBenchmark(LibraryCatalog.HandCoded, true)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public BenchmarkRoots<ITransient1, ITransient2, ITransient3> HandCoded()
    {
        return new(
            new Transient1(),
            new Transient2(),
            new Transient3());
    }
}
