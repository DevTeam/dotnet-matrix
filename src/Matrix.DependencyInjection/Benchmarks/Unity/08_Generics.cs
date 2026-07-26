using Unity;
using Unity.Lifetime;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Generics
{
    private UnityContainer _unity = null!;

    [GlobalSetup(Target = nameof(Unity))]
    public void SetupUnity()
    {
        var container = new UnityContainer();
        container.RegisterType(
            typeof(IGenericService<>),
            typeof(GenericService<>),
            new TransientLifetimeManager());
        container.RegisterType(typeof(GenericRoot<>), new TransientLifetimeManager());
        _unity = container;
    }

    [GlobalCleanup(Target = nameof(Unity))]
    public void CleanupUnity() => _unity.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Unity)]
    public BenchmarkRoots<GenericRoot<int>, GenericRoot<float>, GenericRoot<object>> Unity() =>
        new(
            _unity.Resolve<GenericRoot<int>>(),
            _unity.Resolve<GenericRoot<float>>(),
            _unity.Resolve<GenericRoot<object>>());
}
