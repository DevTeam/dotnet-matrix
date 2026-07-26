using Unity;
using Unity.Lifetime;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Singleton
{
    private UnityContainer _unity = null!;

    [GlobalSetup(Target = nameof(Unity))]
    public void SetupUnity()
    {
        var container = new UnityContainer();
        container.RegisterType<ISingleton1, Singleton1>(new ContainerControlledLifetimeManager());
        container.RegisterType<ISingleton2, Singleton2>(new ContainerControlledLifetimeManager());
        container.RegisterType<ISingleton3, Singleton3>(new ContainerControlledLifetimeManager());
        _unity = container;
    }

    [GlobalCleanup(Target = nameof(Unity))]
    public void CleanupUnity() => _unity.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Unity)]
    public BenchmarkRoots<ISingleton1, ISingleton2, ISingleton3> Unity()
    {
        var first = _unity.Resolve<ISingleton1>();
        var second = _unity.Resolve<ISingleton2>();
        var third = _unity.Resolve<ISingleton3>();
        Validate(LibraryCatalog.Unity, first, second, third);
        return new(first, second, third);
    }
}
