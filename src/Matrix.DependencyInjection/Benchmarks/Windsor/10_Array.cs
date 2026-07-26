using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Array
{
    private WindsorContainer _windsor = null!;

    [GlobalSetup(Target = nameof(Windsor))]
    public void SetupWindsor()
    {
        var container = new WindsorContainer();
        container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel));
        container.Register(
            Component.For<IPlugin>().ImplementedBy<Plugin1>().LifestyleTransient(),
            Component.For<IPlugin>().ImplementedBy<Plugin2>().LifestyleTransient(),
            Component.For<IPlugin>().ImplementedBy<Plugin3>().LifestyleTransient(),
            Component.For<IPlugin>().ImplementedBy<Plugin4>().LifestyleTransient(),
            Component.For<IPlugin>().ImplementedBy<Plugin5>().LifestyleTransient(),
            Component.For<ArrayRoot1>().LifestyleTransient(),
            Component.For<ArrayRoot2>().LifestyleTransient(),
            Component.For<ArrayRoot3>().LifestyleTransient());
        _windsor = container;
    }

    [GlobalCleanup(Target = nameof(Windsor))]
    public void CleanupWindsor() => _windsor.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Windsor)]
    public BenchmarkRoots<ArrayRoot1, ArrayRoot2, ArrayRoot3> Windsor()
    {
        var first = _windsor.Resolve<ArrayRoot1>();
        var second = _windsor.Resolve<ArrayRoot2>();
        var third = _windsor.Resolve<ArrayRoot3>();
        Validate(LibraryCatalog.Windsor, first, second, third);
        return new(first, second, third);
    }
}
