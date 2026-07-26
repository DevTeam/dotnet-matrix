using Grace.DependencyInjection;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Combined
{
    private DependencyInjectionContainer _grace = null!;

    [GlobalSetup(Target = nameof(Grace))]
    public void SetupGrace()
    {
        var container = new DependencyInjectionContainer();
        container.Configure(block =>
        {
            block.Export<CombinedSingleton>().As<ICombinedSingleton>().Lifestyle.Singleton();
            block.Export<CombinedTransient>().As<ICombinedTransient>();
            block.Export<CombinedRoot1>();
            block.Export<CombinedRoot2>();
            block.Export<CombinedRoot3>();
        });
        _grace = container;
    }

    [GlobalCleanup(Target = nameof(Grace))]
    public void CleanupGrace() => _grace.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Grace)]
    public BenchmarkRoots<CombinedRoot1, CombinedRoot2, CombinedRoot3> Grace()
    {
        var first = _grace.Locate<CombinedRoot1>();
        var second = _grace.Locate<CombinedRoot2>();
        var third = _grace.Locate<CombinedRoot3>();
        Validate(LibraryCatalog.Grace, first, second, third);
        return new(first, second, third);
    }
}
