using DryIoc;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Singleton
{
    private Container _dryIoc = null!;

    [GlobalSetup(Target = nameof(DryIoc))]
    public void SetupDryIoc()
    {
        var container = new Container();
        container.Register<ISingleton1, Singleton1>(Reuse.Singleton);
        container.Register<ISingleton2, Singleton2>(Reuse.Singleton);
        container.Register<ISingleton3, Singleton3>(Reuse.Singleton);
        _dryIoc = container;
    }

    [GlobalCleanup(Target = nameof(DryIoc))]
    public void CleanupDryIoc() => _dryIoc.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.DryIoc)]
    public BenchmarkRoots<ISingleton1, ISingleton2, ISingleton3> DryIoc()
    {
        var first = _dryIoc.Resolve<ISingleton1>();
        var second = _dryIoc.Resolve<ISingleton2>();
        var third = _dryIoc.Resolve<ISingleton3>();
        Validate(LibraryCatalog.DryIoc, first, second, third);
        return new(first, second, third);
    }
}
