using Ninject;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Combined
{
    private StandardKernel _ninject = null!;

    [GlobalSetup(Target = nameof(Ninject))]
    public void SetupNinject()
    {
        var kernel = new StandardKernel();
        kernel.Bind<ICombinedSingleton>().To<CombinedSingleton>().InSingletonScope();
        kernel.Bind<ICombinedTransient>().To<CombinedTransient>().InTransientScope();
        kernel.Bind<CombinedRoot1>().ToSelf().InTransientScope();
        kernel.Bind<CombinedRoot2>().ToSelf().InTransientScope();
        kernel.Bind<CombinedRoot3>().ToSelf().InTransientScope();
        _ninject = kernel;
    }

    [GlobalCleanup(Target = nameof(Ninject))]
    public void CleanupNinject() => _ninject.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Ninject)]
    public BenchmarkRoots<CombinedRoot1, CombinedRoot2, CombinedRoot3> Ninject()
    {
        var first = _ninject.Get<CombinedRoot1>();
        var second = _ninject.Get<CombinedRoot2>();
        var third = _ninject.Get<CombinedRoot3>();
        Validate(LibraryCatalog.Ninject, first, second, third);
        return new(first, second, third);
    }
}
