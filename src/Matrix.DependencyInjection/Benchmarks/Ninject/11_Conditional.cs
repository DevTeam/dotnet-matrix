using Ninject;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Conditional
{
    private StandardKernel _ninject = null!;

    [GlobalSetup(Target = nameof(Ninject))]
    public void SetupNinject()
    {
        var kernel = new StandardKernel();
        kernel.Bind<IConditionalService>().To<ConditionalService1>()
            .WhenInjectedInto<ConditionalRoot1>().InTransientScope();
        kernel.Bind<IConditionalService>().To<ConditionalService2>()
            .WhenInjectedInto<ConditionalRoot2>().InTransientScope();
        kernel.Bind<IConditionalService>().To<ConditionalService3>()
            .WhenInjectedInto<ConditionalRoot3>().InTransientScope();
        kernel.Bind<ConditionalRoot1>().ToSelf().InTransientScope();
        kernel.Bind<ConditionalRoot2>().ToSelf().InTransientScope();
        kernel.Bind<ConditionalRoot3>().ToSelf().InTransientScope();
        _ninject = kernel;
    }

    [GlobalCleanup(Target = nameof(Ninject))]
    public void CleanupNinject() => _ninject.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Ninject)]
    public BenchmarkRoots<ConditionalRoot1, ConditionalRoot2, ConditionalRoot3> Ninject()
    {
        var first = _ninject.Get<ConditionalRoot1>();
        var second = _ninject.Get<ConditionalRoot2>();
        var third = _ninject.Get<ConditionalRoot3>();
        Validate(LibraryCatalog.Ninject, first, second, third);
        return new(first, second, third);
    }
}
