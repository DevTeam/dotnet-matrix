// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Combined
{
    private static readonly ICombinedSingleton HandCodedSingleton = new CombinedSingleton();

    [Benchmark(Baseline = true)]
    [LibraryBenchmark(LibraryCatalog.HandCoded, true)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public BenchmarkRoots<CombinedRoot1, CombinedRoot2, CombinedRoot3> HandCoded()
    {
        return new(
            new CombinedRoot1(HandCodedSingleton, new CombinedTransient()),
            new CombinedRoot2(HandCodedSingleton, new CombinedTransient()),
            new CombinedRoot3(HandCodedSingleton, new CombinedTransient()));
    }
}
