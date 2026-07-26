using Unity;
using Unity.Lifetime;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.DependencyInjection.Benchmarks;

public partial class PerResolve
{
    private UnityContainer _unity = null!;

    [GlobalSetup(Target = nameof(Unity))]
    public void SetupUnity()
    {
        var container = new UnityContainer();
        container.RegisterType<IPerResolveDependency, PerResolveDependency>(
            new PerResolveLifetimeManager());
        container.RegisterType<PerResolveRoot>(new TransientLifetimeManager());
        _unity = container;
    }

    [GlobalCleanup(Target = nameof(Unity))]
    public void CleanupUnity() => _unity.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Unity)]
    public PerResolveRoot Unity()
    {
        var root = _unity.Resolve<PerResolveRoot>();
        Validate(LibraryCatalog.Unity, root);
        return root;
    }
}
