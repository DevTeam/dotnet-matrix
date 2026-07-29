// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Array
{
    [Benchmark(Baseline = true)]
    [LibraryBenchmark(LibraryCatalog.HandCoded)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public BenchmarkRoots<ArrayRoot1, ArrayRoot2, ArrayRoot3> HandCoded()
    {
        var first = new ArrayRoot1(CreatePlugins());
        var second = new ArrayRoot2(CreatePlugins());
        var third = new ArrayRoot3(CreatePlugins());
        Validate(LibraryCatalog.HandCoded, first, second, third);
        return new(first, second, third);
    }

    private static IPlugin[] CreatePlugins() =>
    [
        new Plugin1(),
        new Plugin2(),
        new Plugin3(),
        new Plugin4(),
        new Plugin5()
    ];
}
