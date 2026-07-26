using Grace.DependencyInjection;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Singleton
{
    private DependencyInjectionContainer _grace = null!;

    [GlobalSetup(Target = nameof(Grace))]
    public void SetupGrace()
    {
        var container = new DependencyInjectionContainer();
        container.Configure(block =>
        {
            block.Export<Singleton1>().As<ISingleton1>().Lifestyle.Singleton();
            block.Export<Singleton2>().As<ISingleton2>().Lifestyle.Singleton();
            block.Export<Singleton3>().As<ISingleton3>().Lifestyle.Singleton();
        });
        _grace = container;
    }

    [GlobalCleanup(Target = nameof(Grace))]
    public void CleanupGrace() => _grace.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Grace)]
    public BenchmarkRoots<ISingleton1, ISingleton2, ISingleton3> Grace()
    {
        var first = _grace.Locate<ISingleton1>();
        var second = _grace.Locate<ISingleton2>();
        var third = _grace.Locate<ISingleton3>();
        Validate(LibraryCatalog.Grace, first, second, third);
        return new(first, second, third);
    }
}
