using Castle.MicroKernel.Registration;
using Castle.Windsor;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.DependencyInjection.Benchmarks;

public partial class PerResolve
{
    private WindsorContainer _windsor = null!;

    [GlobalSetup(Target = nameof(Windsor))]
    public void SetupWindsor()
    {
        var container = new WindsorContainer();
        container.Register(
            Component.For<IPerResolveDependency>()
                .ImplementedBy<PerResolveDependency>()
                .LifestyleBoundTo<PerResolveRoot>(),
            Component.For<PerResolveRoot>().LifestyleTransient());
        _windsor = container;
    }

    [GlobalCleanup(Target = nameof(Windsor))]
    public void CleanupWindsor() => _windsor.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Windsor)]
    public PerResolveRoot Windsor()
    {
        var root = _windsor.Resolve<PerResolveRoot>();
        Validate(LibraryCatalog.Windsor, root);
        return root;
    }
}
