using DryIoc;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Array
{
    private Container _dryIoc = null!;

    [GlobalSetup(Target = nameof(DryIoc))]
    public void SetupDryIoc()
    {
        var container = new Container();
        container.Register<IPlugin, Plugin1>(Reuse.Transient);
        container.Register<IPlugin, Plugin2>(Reuse.Transient);
        container.Register<IPlugin, Plugin3>(Reuse.Transient);
        container.Register<IPlugin, Plugin4>(Reuse.Transient);
        container.Register<IPlugin, Plugin5>(Reuse.Transient);
        container.Register<ArrayRoot1>(Reuse.Transient);
        container.Register<ArrayRoot2>(Reuse.Transient);
        container.Register<ArrayRoot3>(Reuse.Transient);
        _dryIoc = container;
    }

    [GlobalCleanup(Target = nameof(DryIoc))]
    public void CleanupDryIoc() => _dryIoc.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.DryIoc)]
    public BenchmarkRoots<ArrayRoot1, ArrayRoot2, ArrayRoot3> DryIoc()
    {
        var first = _dryIoc.Resolve<ArrayRoot1>();
        var second = _dryIoc.Resolve<ArrayRoot2>();
        var third = _dryIoc.Resolve<ArrayRoot3>();
        Validate(LibraryCatalog.DryIoc, first, second, third);
        return new(first, second, third);
    }
}
