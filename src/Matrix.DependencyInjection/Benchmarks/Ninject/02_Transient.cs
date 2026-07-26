using Ninject;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Transient
{
    private StandardKernel _ninject = null!;

    [GlobalSetup(Target = nameof(Ninject))]
    public void SetupNinject()
    {
        var kernel = new StandardKernel();
        kernel.Bind<ITransient1>().To<Transient1>().InTransientScope();
        kernel.Bind<ITransient2>().To<Transient2>().InTransientScope();
        kernel.Bind<ITransient3>().To<Transient3>().InTransientScope();
        _ninject = kernel;
    }

    [GlobalCleanup(Target = nameof(Ninject))]
    public void CleanupNinject() => _ninject.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Ninject)]
    public BenchmarkRoots<ITransient1, ITransient2, ITransient3> Ninject()
    {
        var first = _ninject.Get<ITransient1>();
        var second = _ninject.Get<ITransient2>();
        var third = _ninject.Get<ITransient3>();
        Validate(LibraryCatalog.Ninject, first);
        return new(first, second, third);
    }
}
