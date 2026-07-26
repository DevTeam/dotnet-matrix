using Ninject;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Complex
{
    private StandardKernel _ninject = null!;

    [GlobalSetup(Target = nameof(Ninject))]
    public void SetupNinject()
    {
        var kernel = new StandardKernel();
        kernel.Bind<IFirstService>().To<FirstService>().InSingletonScope();
        kernel.Bind<ISecondService>().To<SecondService>().InSingletonScope();
        kernel.Bind<IThirdService>().To<ThirdService>().InSingletonScope();
        kernel.Bind<SubObject1>().ToSelf().InTransientScope();
        kernel.Bind<SubObject2>().ToSelf().InTransientScope();
        kernel.Bind<SubObject3>().ToSelf().InTransientScope();
        kernel.Bind<ComplexRoot1>().ToSelf().InTransientScope();
        kernel.Bind<ComplexRoot2>().ToSelf().InTransientScope();
        kernel.Bind<ComplexRoot3>().ToSelf().InTransientScope();
        _ninject = kernel;
    }

    [GlobalCleanup(Target = nameof(Ninject))]
    public void CleanupNinject() => _ninject.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Ninject)]
    public BenchmarkRoots<ComplexRoot1, ComplexRoot2, ComplexRoot3> Ninject() =>
        new(
            _ninject.Get<ComplexRoot1>(),
            _ninject.Get<ComplexRoot2>(),
            _ninject.Get<ComplexRoot3>());
}
