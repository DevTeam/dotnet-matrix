// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Enumerable
{
    // A field, not a per-call CreatePlugins(): the roots share one lazy
    // sequence the way a container's own collection registration would,
    // instead of each root paying for its own iterator. The C# iterator
    // still hands out a fresh enumerator - and therefore new plugins - on
    // every enumeration, so sharing the field costs nothing in freshness.
    private static readonly IEnumerable<IPlugin> Plugins = CreatePlugins();

    [Benchmark(Baseline = true)]
    [LibraryBenchmark(LibraryCatalog.HandCoded)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public BenchmarkRoots<EnumerableRoot1, EnumerableRoot2, EnumerableRoot3> HandCoded()
    {
        var first = new EnumerableRoot1(Plugins);
        var second = new EnumerableRoot2(Plugins);
        var third = new EnumerableRoot3(Plugins);
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
