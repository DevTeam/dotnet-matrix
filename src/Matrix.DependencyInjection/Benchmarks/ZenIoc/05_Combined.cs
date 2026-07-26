using ZenIoc;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Combined
{
    private IIocContainer _zenIoc = null!;

    [GlobalSetup(Target = nameof(ZenIoc))]
    public void SetupZenIoc()
    {
        IIocContainer container = new IocContainer();
        container.Register<ICombinedSingleton, CombinedSingleton>().SingleInstance();
        container.Register<ICombinedTransient, CombinedTransient>();
        container.Register<CombinedRoot1>();
        container.Register<CombinedRoot2>();
        container.Register<CombinedRoot3>();
        container.Compile();
        _zenIoc = container;
    }

    [GlobalCleanup(Target = nameof(ZenIoc))]
    public void CleanupZenIoc() => _zenIoc.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ZenIoc)]
    public BenchmarkRoots<CombinedRoot1, CombinedRoot2, CombinedRoot3> ZenIoc()
    {
        var first = _zenIoc.Resolve<CombinedRoot1>();
        var second = _zenIoc.Resolve<CombinedRoot2>();
        var third = _zenIoc.Resolve<CombinedRoot3>();
        Validate(LibraryCatalog.ZenIoc, first, second, third);
        return new(first, second, third);
    }
}
