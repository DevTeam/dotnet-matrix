using Castle.MicroKernel.Registration;
using Castle.Windsor;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Combined
{
    private WindsorContainer _windsor = null!;

    [GlobalSetup(Target = nameof(Windsor))]
    public void SetupWindsor()
    {
        var container = new WindsorContainer();
        container.Register(
            Component.For<ICombinedSingleton>().ImplementedBy<CombinedSingleton>().LifestyleSingleton(),
            Component.For<ICombinedTransient>().ImplementedBy<CombinedTransient>().LifestyleTransient(),
            Component.For<CombinedRoot1>().LifestyleTransient(),
            Component.For<CombinedRoot2>().LifestyleTransient(),
            Component.For<CombinedRoot3>().LifestyleTransient());
        _windsor = container;
    }

    [GlobalCleanup(Target = nameof(Windsor))]
    public void CleanupWindsor() => _windsor.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Windsor)]
    public BenchmarkRoots<CombinedRoot1, CombinedRoot2, CombinedRoot3> Windsor()
    {
        var first = _windsor.Resolve<CombinedRoot1>();
        var second = _windsor.Resolve<CombinedRoot2>();
        var third = _windsor.Resolve<CombinedRoot3>();
        Validate(LibraryCatalog.Windsor, first, second, third);
        return new(first, second, third);
    }
}
