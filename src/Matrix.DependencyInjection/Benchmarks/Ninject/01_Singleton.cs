using Ninject;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Singleton
{
    private StandardKernel _ninject = null!;

    [GlobalSetup(Target = nameof(Ninject))]
    public void SetupNinject()
    {
        var kernel = new StandardKernel();
        kernel.Bind<ISingleton1>().To<Singleton1>().InSingletonScope();
        kernel.Bind<ISingleton2>().To<Singleton2>().InSingletonScope();
        kernel.Bind<ISingleton3>().To<Singleton3>().InSingletonScope();
        _ninject = kernel;
    }

    [GlobalCleanup(Target = nameof(Ninject))]
    public void CleanupNinject() => _ninject.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Ninject)]
    public BenchmarkRoots<ISingleton1, ISingleton2, ISingleton3> Ninject()
    {
        var first = _ninject.Get<ISingleton1>();
        var second = _ninject.Get<ISingleton2>();
        var third = _ninject.Get<ISingleton3>();
        Validate(LibraryCatalog.Ninject, first, second, third);
        return new(first, second, third);
    }
}
