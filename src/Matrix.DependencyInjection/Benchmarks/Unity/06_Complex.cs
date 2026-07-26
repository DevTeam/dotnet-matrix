using Unity;
using Unity.Lifetime;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Complex
{
    private UnityContainer _unity = null!;

    [GlobalSetup(Target = nameof(Unity))]
    public void SetupUnity()
    {
        var container = new UnityContainer();
        container.RegisterType<IFirstService, FirstService>(new ContainerControlledLifetimeManager());
        container.RegisterType<ISecondService, SecondService>(new ContainerControlledLifetimeManager());
        container.RegisterType<IThirdService, ThirdService>(new ContainerControlledLifetimeManager());
        container.RegisterType<SubObject1>(new TransientLifetimeManager());
        container.RegisterType<SubObject2>(new TransientLifetimeManager());
        container.RegisterType<SubObject3>(new TransientLifetimeManager());
        container.RegisterType<ComplexRoot1>(new TransientLifetimeManager());
        container.RegisterType<ComplexRoot2>(new TransientLifetimeManager());
        container.RegisterType<ComplexRoot3>(new TransientLifetimeManager());
        _unity = container;
    }

    [GlobalCleanup(Target = nameof(Unity))]
    public void CleanupUnity() => _unity.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Unity)]
    public BenchmarkRoots<ComplexRoot1, ComplexRoot2, ComplexRoot3> Unity() =>
        new(
            _unity.Resolve<ComplexRoot1>(),
            _unity.Resolve<ComplexRoot2>(),
            _unity.Resolve<ComplexRoot3>());
}
