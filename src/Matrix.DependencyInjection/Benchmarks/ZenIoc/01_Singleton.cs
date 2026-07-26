using ZenIoc;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Singleton
{
    private IIocContainer _zenIoc = null!;

    [GlobalSetup(Target = nameof(ZenIoc))]
    public void SetupZenIoc()
    {
        IIocContainer container = new IocContainer();
        container.Register<ISingleton1, Singleton1>().SingleInstance();
        container.Register<ISingleton2, Singleton2>().SingleInstance();
        container.Register<ISingleton3, Singleton3>().SingleInstance();
        container.Compile();
        _zenIoc = container;
    }

    [GlobalCleanup(Target = nameof(ZenIoc))]
    public void CleanupZenIoc() => _zenIoc.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ZenIoc)]
    public BenchmarkRoots<ISingleton1, ISingleton2, ISingleton3> ZenIoc()
    {
        var first = _zenIoc.Resolve<ISingleton1>();
        var second = _zenIoc.Resolve<ISingleton2>();
        var third = _zenIoc.Resolve<ISingleton3>();
        Validate(LibraryCatalog.ZenIoc, first, second, third);
        return new(first, second, third);
    }
}
