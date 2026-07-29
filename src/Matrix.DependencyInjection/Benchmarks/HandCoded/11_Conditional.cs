// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Conditional
{
    [Benchmark(Baseline = true)]
    [LibraryBenchmark(LibraryCatalog.HandCoded)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public BenchmarkRoots<ConditionalRoot1, ConditionalRoot2, ConditionalRoot3> HandCoded()
    {
        return new(
            new ConditionalRoot1(new ConditionalService1()),
            new ConditionalRoot2(new ConditionalService2()),
            new ConditionalRoot3(new ConditionalService3()));
    }
}
