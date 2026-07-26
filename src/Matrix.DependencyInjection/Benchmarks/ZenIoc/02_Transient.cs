using ZenIoc;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Transient
{
    private IIocContainer _zenIoc = null!;

    [GlobalSetup(Target = nameof(ZenIoc))]
    public void SetupZenIoc()
    {
        IIocContainer container = new IocContainer();
        container.Register<ITransient1, Transient1>();
        container.Register<ITransient2, Transient2>();
        container.Register<ITransient3, Transient3>();
        container.Compile();
        _zenIoc = container;
    }

    [GlobalCleanup(Target = nameof(ZenIoc))]
    public void CleanupZenIoc() => _zenIoc.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ZenIoc)]
    public BenchmarkRoots<ITransient1, ITransient2, ITransient3> ZenIoc()
    {
        var first = _zenIoc.Resolve<ITransient1>();
        var second = _zenIoc.Resolve<ITransient2>();
        var third = _zenIoc.Resolve<ITransient3>();
        Validate(LibraryCatalog.ZenIoc, first);
        return new(first, second, third);
    }
}
