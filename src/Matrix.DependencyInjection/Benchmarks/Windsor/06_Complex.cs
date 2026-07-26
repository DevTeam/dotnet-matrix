using Castle.MicroKernel.Registration;
using Castle.Windsor;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Complex
{
    private WindsorContainer _windsor = null!;

    [GlobalSetup(Target = nameof(Windsor))]
    public void SetupWindsor()
    {
        var container = new WindsorContainer();
        container.Register(
            Component.For<IFirstService>().ImplementedBy<FirstService>().LifestyleSingleton(),
            Component.For<ISecondService>().ImplementedBy<SecondService>().LifestyleSingleton(),
            Component.For<IThirdService>().ImplementedBy<ThirdService>().LifestyleSingleton(),
            Component.For<SubObject1>().LifestyleTransient(),
            Component.For<SubObject2>().LifestyleTransient(),
            Component.For<SubObject3>().LifestyleTransient(),
            Component.For<ComplexRoot1>().LifestyleTransient(),
            Component.For<ComplexRoot2>().LifestyleTransient(),
            Component.For<ComplexRoot3>().LifestyleTransient());
        _windsor = container;
    }

    [GlobalCleanup(Target = nameof(Windsor))]
    public void CleanupWindsor() => _windsor.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Windsor)]
    public BenchmarkRoots<ComplexRoot1, ComplexRoot2, ComplexRoot3> Windsor() =>
        new(
            _windsor.Resolve<ComplexRoot1>(),
            _windsor.Resolve<ComplexRoot2>(),
            _windsor.Resolve<ComplexRoot3>());
}
