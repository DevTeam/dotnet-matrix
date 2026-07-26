using DryIoc;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Combined
{
    private Container _dryIoc = null!;

    [GlobalSetup(Target = nameof(DryIoc))]
    public void SetupDryIoc()
    {
        var container = new Container();
        container.Register<ICombinedSingleton, CombinedSingleton>(Reuse.Singleton);
        container.Register<ICombinedTransient, CombinedTransient>(Reuse.Transient);
        container.Register<CombinedRoot1>(Reuse.Transient);
        container.Register<CombinedRoot2>(Reuse.Transient);
        container.Register<CombinedRoot3>(Reuse.Transient);
        _dryIoc = container;
    }

    [GlobalCleanup(Target = nameof(DryIoc))]
    public void CleanupDryIoc() => _dryIoc.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.DryIoc)]
    public BenchmarkRoots<CombinedRoot1, CombinedRoot2, CombinedRoot3> DryIoc()
    {
        var first = _dryIoc.Resolve<CombinedRoot1>();
        var second = _dryIoc.Resolve<CombinedRoot2>();
        var third = _dryIoc.Resolve<CombinedRoot3>();
        Validate(LibraryCatalog.DryIoc, first, second, third);
        return new(first, second, third);
    }
}
