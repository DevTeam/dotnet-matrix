// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Property
{
    [Benchmark(Baseline = true)]
    [LibraryBenchmark(LibraryCatalog.HandCoded, true)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public BenchmarkRoots<PropertyRoot1, PropertyRoot2, PropertyRoot3> HandCoded()
    {
        return new(
            new PropertyRoot1
            {
                ServiceA = new PropertyServiceA(),
                ServiceB = new PropertyServiceB(),
                ServiceC = new PropertyServiceC()
            },
            new PropertyRoot2
            {
                ServiceA = new PropertyServiceA(),
                ServiceB = new PropertyServiceB(),
                ServiceC = new PropertyServiceC()
            },
            new PropertyRoot3
            {
                ServiceA = new PropertyServiceA(),
                ServiceB = new PropertyServiceB(),
                ServiceC = new PropertyServiceC()
            });
    }
}
