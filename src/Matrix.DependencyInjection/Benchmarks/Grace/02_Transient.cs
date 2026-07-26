using Grace.DependencyInjection;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Transient
{
    private DependencyInjectionContainer _grace = null!;

    [GlobalSetup(Target = nameof(Grace))]
    public void SetupGrace()
    {
        var container = new DependencyInjectionContainer();
        container.Configure(block =>
        {
            block.Export<Transient1>().As<ITransient1>();
            block.Export<Transient2>().As<ITransient2>();
            block.Export<Transient3>().As<ITransient3>();
        });
        _grace = container;
    }

    [GlobalCleanup(Target = nameof(Grace))]
    public void CleanupGrace() => _grace.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Grace)]
    public BenchmarkRoots<ITransient1, ITransient2, ITransient3> Grace()
    {
        var first = _grace.Locate<ITransient1>();
        var second = _grace.Locate<ITransient2>();
        var third = _grace.Locate<ITransient3>();
        Validate(LibraryCatalog.Grace, first);
        return new(first, second, third);
    }
}
