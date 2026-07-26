using DryIoc;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Transient
{
    private Container _dryIoc = null!;

    [GlobalSetup(Target = nameof(DryIoc))]
    public void SetupDryIoc()
    {
        var container = new Container();
        container.Register<ITransient1, Transient1>(Reuse.Transient);
        container.Register<ITransient2, Transient2>(Reuse.Transient);
        container.Register<ITransient3, Transient3>(Reuse.Transient);
        _dryIoc = container;
    }

    [GlobalCleanup(Target = nameof(DryIoc))]
    public void CleanupDryIoc() => _dryIoc.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.DryIoc)]
    public BenchmarkRoots<ITransient1, ITransient2, ITransient3> DryIoc()
    {
        var first = _dryIoc.Resolve<ITransient1>();
        var second = _dryIoc.Resolve<ITransient2>();
        var third = _dryIoc.Resolve<ITransient3>();
        Validate(LibraryCatalog.DryIoc, first);
        return new(first, second, third);
    }
}
