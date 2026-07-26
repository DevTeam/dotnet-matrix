using Castle.MicroKernel.Registration;
using Castle.Windsor;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Conditional
{
    private WindsorContainer _windsor = null!;

    [GlobalSetup(Target = nameof(Windsor))]
    public void SetupWindsor()
    {
        var container = new WindsorContainer();
        container.Register(
            Component.For<IConditionalService>()
                .ImplementedBy<ConditionalService1>()
                .Named("1")
                .LifestyleTransient(),
            Component.For<IConditionalService>()
                .ImplementedBy<ConditionalService2>()
                .Named("2")
                .LifestyleTransient(),
            Component.For<IConditionalService>()
                .ImplementedBy<ConditionalService3>()
                .Named("3")
                .LifestyleTransient(),
            Component.For<ConditionalRoot1>()
                .DependsOn(ServiceOverride.ForKey<IConditionalService>().Eq("1"))
                .LifestyleTransient(),
            Component.For<ConditionalRoot2>()
                .DependsOn(ServiceOverride.ForKey<IConditionalService>().Eq("2"))
                .LifestyleTransient(),
            Component.For<ConditionalRoot3>()
                .DependsOn(ServiceOverride.ForKey<IConditionalService>().Eq("3"))
                .LifestyleTransient());
        _windsor = container;
    }

    [GlobalCleanup(Target = nameof(Windsor))]
    public void CleanupWindsor() => _windsor.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Windsor)]
    public BenchmarkRoots<ConditionalRoot1, ConditionalRoot2, ConditionalRoot3> Windsor()
    {
        var first = _windsor.Resolve<ConditionalRoot1>();
        var second = _windsor.Resolve<ConditionalRoot2>();
        var third = _windsor.Resolve<ConditionalRoot3>();
        Validate(LibraryCatalog.Windsor, first, second, third);
        return new(first, second, third);
    }
}
