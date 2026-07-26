using Castle.MicroKernel.Lifestyle;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Scoped
{
    private WindsorContainer _windsor = null!;

    [GlobalSetup(Target = nameof(Windsor))]
    public void SetupWindsor()
    {
        var container = new WindsorContainer();
        container.Register(
            Component.For<IScopedDependency>().ImplementedBy<ScopedDependency>().LifestyleScoped(),
            Component.For<ScopedRoot>().LifestyleTransient());
        _windsor = container;
    }

    [GlobalCleanup(Target = nameof(Windsor))]
    public void CleanupWindsor() => _windsor.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Windsor)]
    public BenchmarkRoots<ScopedRoot, ScopedRoot> Windsor()
    {
        using var scope = _windsor.BeginScope();
        var first = _windsor.Resolve<ScopedRoot>();
        var second = _windsor.Resolve<ScopedRoot>();
        Validate(LibraryCatalog.Windsor, first, second);
        return new(first, second);
    }
}
