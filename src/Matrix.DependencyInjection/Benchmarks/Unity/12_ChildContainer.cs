using Unity;
using Unity.Lifetime;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class ChildContainer
{
    private IUnityContainer _unity = null!;

    [GlobalSetup(Target = nameof(Unity))]
    public void SetupUnity()
    {
        var container = new UnityContainer();
        container.RegisterType<IChildValue, ParentValue>(new TransientLifetimeManager());
        container.RegisterType<ChildRoot>(new TransientLifetimeManager());
        _unity = container;
    }

    [GlobalCleanup(Target = nameof(Unity))]
    public void CleanupUnity() => _unity.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Unity)]
    public BenchmarkRoots<ChildRoot, ChildRoot> Unity()
    {
        var parent = _unity.Resolve<ChildRoot>();
        using var child = _unity.CreateChildContainer();
        child.RegisterType<IChildValue, ChildValue>(new TransientLifetimeManager());
        var root = child.Resolve<ChildRoot>();
        Validate(LibraryCatalog.Unity, parent, root);
        return new(parent, root);
    }
}
