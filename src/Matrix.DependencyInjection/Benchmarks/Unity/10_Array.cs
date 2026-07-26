using Unity;
using Unity.Lifetime;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Array
{
    private UnityContainer _unity = null!;

    [GlobalSetup(Target = nameof(Unity))]
    public void SetupUnity()
    {
        var container = new UnityContainer();
        container.RegisterType<IPlugin, Plugin1>("1", new TransientLifetimeManager());
        container.RegisterType<IPlugin, Plugin2>("2", new TransientLifetimeManager());
        container.RegisterType<IPlugin, Plugin3>("3", new TransientLifetimeManager());
        container.RegisterType<IPlugin, Plugin4>("4", new TransientLifetimeManager());
        container.RegisterType<IPlugin, Plugin5>("5", new TransientLifetimeManager());
        container.RegisterType<ArrayRoot1>(new TransientLifetimeManager());
        container.RegisterType<ArrayRoot2>(new TransientLifetimeManager());
        container.RegisterType<ArrayRoot3>(new TransientLifetimeManager());
        _unity = container;
    }

    [GlobalCleanup(Target = nameof(Unity))]
    public void CleanupUnity() => _unity.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Unity)]
    public BenchmarkRoots<ArrayRoot1, ArrayRoot2, ArrayRoot3> Unity()
    {
        var first = _unity.Resolve<ArrayRoot1>();
        var second = _unity.Resolve<ArrayRoot2>();
        var third = _unity.Resolve<ArrayRoot3>();
        Validate(LibraryCatalog.Unity, first, second, third);
        return new(first, second, third);
    }
}
