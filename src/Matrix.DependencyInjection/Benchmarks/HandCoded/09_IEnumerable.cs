// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Enumerable
{
    [Benchmark(Baseline = true)]
    [LibraryBenchmark(LibraryCatalog.HandCoded)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public BenchmarkRoots<EnumerableRoot1, EnumerableRoot2, EnumerableRoot3> HandCoded()
    {
        var first = new EnumerableRoot1(CreatePlugins());
        var second = new EnumerableRoot2(CreatePlugins());
        var third = new EnumerableRoot3(CreatePlugins());
        Validate(LibraryCatalog.HandCoded, first, second, third);
        return new(first, second, third);
    }

    private static IEnumerable<IPlugin> CreatePlugins()
    {
        yield return new Plugin1();
        yield return new Plugin2();
        yield return new Plugin3();
        yield return new Plugin4();
        yield return new Plugin5();
    }
}
