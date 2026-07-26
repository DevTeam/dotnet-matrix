using Unity;
using Unity.Lifetime;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Combined
{
    private UnityContainer _unity = null!;

    [GlobalSetup(Target = nameof(Unity))]
    public void SetupUnity()
    {
        var container = new UnityContainer();
        container.RegisterType<ICombinedSingleton, CombinedSingleton>(
            new ContainerControlledLifetimeManager());
        container.RegisterType<ICombinedTransient, CombinedTransient>(new TransientLifetimeManager());
        container.RegisterType<CombinedRoot1>(new TransientLifetimeManager());
        container.RegisterType<CombinedRoot2>(new TransientLifetimeManager());
        container.RegisterType<CombinedRoot3>(new TransientLifetimeManager());
        _unity = container;
    }

    [GlobalCleanup(Target = nameof(Unity))]
    public void CleanupUnity() => _unity.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Unity)]
    public BenchmarkRoots<CombinedRoot1, CombinedRoot2, CombinedRoot3> Unity()
    {
        var first = _unity.Resolve<CombinedRoot1>();
        var second = _unity.Resolve<CombinedRoot2>();
        var third = _unity.Resolve<CombinedRoot3>();
        Validate(LibraryCatalog.Unity, first, second, third);
        return new(first, second, third);
    }
}
