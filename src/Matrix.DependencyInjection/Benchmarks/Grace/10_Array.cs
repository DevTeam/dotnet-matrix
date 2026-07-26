using Grace.DependencyInjection;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Array
{
    private DependencyInjectionContainer _grace = null!;

    [GlobalSetup(Target = nameof(Grace))]
    public void SetupGrace()
    {
        var container = new DependencyInjectionContainer();
        container.Configure(block =>
        {
            block.Export<Plugin1>().As<IPlugin>();
            block.Export<Plugin2>().As<IPlugin>();
            block.Export<Plugin3>().As<IPlugin>();
            block.Export<Plugin4>().As<IPlugin>();
            block.Export<Plugin5>().As<IPlugin>();
            block.Export<ArrayRoot1>();
            block.Export<ArrayRoot2>();
            block.Export<ArrayRoot3>();
        });
        _grace = container;
    }

    [GlobalCleanup(Target = nameof(Grace))]
    public void CleanupGrace() => _grace.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Grace)]
    public BenchmarkRoots<ArrayRoot1, ArrayRoot2, ArrayRoot3> Grace()
    {
        var first = _grace.Locate<ArrayRoot1>();
        var second = _grace.Locate<ArrayRoot2>();
        var third = _grace.Locate<ArrayRoot3>();
        Validate(LibraryCatalog.Grace, first, second, third);
        return new(first, second, third);
    }
}
