using Castle.MicroKernel.Registration;
using Castle.Windsor;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Property
{
    private WindsorContainer _windsor = null!;

    [GlobalSetup(Target = nameof(Windsor))]
    public void SetupWindsor()
    {
        var container = new WindsorContainer();
        container.Register(
            Component.For<PropertyServiceA>().LifestyleTransient(),
            Component.For<PropertyServiceB>().LifestyleTransient(),
            Component.For<PropertyServiceC>().LifestyleTransient(),
            Component.For<PropertyRoot1>().LifestyleTransient(),
            Component.For<PropertyRoot2>().LifestyleTransient(),
            Component.For<PropertyRoot3>().LifestyleTransient());
        _windsor = container;
    }

    [GlobalCleanup(Target = nameof(Windsor))]
    public void CleanupWindsor() => _windsor.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Windsor)]
    public BenchmarkRoots<PropertyRoot1, PropertyRoot2, PropertyRoot3> Windsor()
    {
        var first = _windsor.Resolve<PropertyRoot1>();
        var second = _windsor.Resolve<PropertyRoot2>();
        var third = _windsor.Resolve<PropertyRoot3>();
        Validate(LibraryCatalog.Windsor, first, second, third);
        return new(first, second, third);
    }
}
