using Unity;
using Unity.Lifetime;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Transient
{
    private UnityContainer _unity = null!;

    [GlobalSetup(Target = nameof(Unity))]
    public void SetupUnity()
    {
        var container = new UnityContainer();
        container.RegisterType<ITransient1, Transient1>(new TransientLifetimeManager());
        container.RegisterType<ITransient2, Transient2>(new TransientLifetimeManager());
        container.RegisterType<ITransient3, Transient3>(new TransientLifetimeManager());
        _unity = container;
    }

    [GlobalCleanup(Target = nameof(Unity))]
    public void CleanupUnity() => _unity.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Unity)]
    public BenchmarkRoots<ITransient1, ITransient2, ITransient3> Unity()
    {
        var first = _unity.Resolve<ITransient1>();
        var second = _unity.Resolve<ITransient2>();
        var third = _unity.Resolve<ITransient3>();
        Validate(LibraryCatalog.Unity, first);
        return new(first, second, third);
    }
}
