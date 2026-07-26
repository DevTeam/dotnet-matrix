using Castle.MicroKernel.Registration;
using Castle.Windsor;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Transient
{
    private WindsorContainer _windsor = null!;

    [GlobalSetup(Target = nameof(Windsor))]
    public void SetupWindsor()
    {
        var container = new WindsorContainer();
        container.Register(
            Component.For<ITransient1>().ImplementedBy<Transient1>().LifestyleTransient(),
            Component.For<ITransient2>().ImplementedBy<Transient2>().LifestyleTransient(),
            Component.For<ITransient3>().ImplementedBy<Transient3>().LifestyleTransient());
        _windsor = container;
    }

    [GlobalCleanup(Target = nameof(Windsor))]
    public void CleanupWindsor() => _windsor.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Windsor)]
    public BenchmarkRoots<ITransient1, ITransient2, ITransient3> Windsor()
    {
        var first = _windsor.Resolve<ITransient1>();
        var second = _windsor.Resolve<ITransient2>();
        var third = _windsor.Resolve<ITransient3>();
        Validate(LibraryCatalog.Windsor, first);
        return new(first, second, third);
    }
}
