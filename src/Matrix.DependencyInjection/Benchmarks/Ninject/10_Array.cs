using Ninject;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Array
{
    private StandardKernel _ninject = null!;

    [GlobalSetup(Target = nameof(Ninject))]
    public void SetupNinject()
    {
        var kernel = new StandardKernel();
        kernel.Bind<IPlugin>().To<Plugin1>().InTransientScope();
        kernel.Bind<IPlugin>().To<Plugin2>().InTransientScope();
        kernel.Bind<IPlugin>().To<Plugin3>().InTransientScope();
        kernel.Bind<IPlugin>().To<Plugin4>().InTransientScope();
        kernel.Bind<IPlugin>().To<Plugin5>().InTransientScope();
        kernel.Bind<ArrayRoot1>().ToSelf().InTransientScope();
        kernel.Bind<ArrayRoot2>().ToSelf().InTransientScope();
        kernel.Bind<ArrayRoot3>().ToSelf().InTransientScope();
        _ninject = kernel;
    }

    [GlobalCleanup(Target = nameof(Ninject))]
    public void CleanupNinject() => _ninject.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Ninject)]
    public BenchmarkRoots<ArrayRoot1, ArrayRoot2, ArrayRoot3> Ninject()
    {
        var first = _ninject.Get<ArrayRoot1>();
        var second = _ninject.Get<ArrayRoot2>();
        var third = _ninject.Get<ArrayRoot3>();
        Validate(LibraryCatalog.Ninject, first, second, third);
        return new(first, second, third);
    }
}
