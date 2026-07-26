using Unity;
using Unity.Lifetime;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Scoped
{
    private IUnityContainer _unity = null!;

    [GlobalSetup(Target = nameof(Unity))]
    public void SetupUnity()
    {
        var container = new UnityContainer();
        container.RegisterType<IScopedDependency, ScopedDependency>(new HierarchicalLifetimeManager());
        container.RegisterType<ScopedRoot>(new TransientLifetimeManager());
        _unity = container;
    }

    [GlobalCleanup(Target = nameof(Unity))]
    public void CleanupUnity() => _unity.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Unity)]
    public BenchmarkRoots<ScopedRoot, ScopedRoot> Unity()
    {
        using var scope = _unity.CreateChildContainer();
        var first = scope.Resolve<ScopedRoot>();
        var second = scope.Resolve<ScopedRoot>();
        Validate(LibraryCatalog.Unity, first, second);
        return new(first, second);
    }
}
