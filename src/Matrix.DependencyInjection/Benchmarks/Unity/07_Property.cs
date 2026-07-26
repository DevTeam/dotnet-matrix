using Unity;
using Unity.Injection;
using Unity.Lifetime;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Property
{
    private UnityContainer _unity = null!;

    [GlobalSetup(Target = nameof(Unity))]
    public void SetupUnity()
    {
        var container = new UnityContainer();
        container.RegisterType<PropertyServiceA>(new TransientLifetimeManager());
        container.RegisterType<PropertyServiceB>(new TransientLifetimeManager());
        container.RegisterType<PropertyServiceC>(new TransientLifetimeManager());
        container.RegisterType<PropertyRoot1>(
            new TransientLifetimeManager(),
            new InjectionProperty(nameof(PropertyRoot1.ServiceA)),
            new InjectionProperty(nameof(PropertyRoot1.ServiceB)),
            new InjectionProperty(nameof(PropertyRoot1.ServiceC)));
        container.RegisterType<PropertyRoot2>(
            new TransientLifetimeManager(),
            new InjectionProperty(nameof(PropertyRoot2.ServiceA)),
            new InjectionProperty(nameof(PropertyRoot2.ServiceB)),
            new InjectionProperty(nameof(PropertyRoot2.ServiceC)));
        container.RegisterType<PropertyRoot3>(
            new TransientLifetimeManager(),
            new InjectionProperty(nameof(PropertyRoot3.ServiceA)),
            new InjectionProperty(nameof(PropertyRoot3.ServiceB)),
            new InjectionProperty(nameof(PropertyRoot3.ServiceC)));
        _unity = container;
    }

    [GlobalCleanup(Target = nameof(Unity))]
    public void CleanupUnity() => _unity.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Unity)]
    public BenchmarkRoots<PropertyRoot1, PropertyRoot2, PropertyRoot3> Unity()
    {
        var first = _unity.Resolve<PropertyRoot1>();
        var second = _unity.Resolve<PropertyRoot2>();
        var third = _unity.Resolve<PropertyRoot3>();
        Validate(LibraryCatalog.Unity, first, second, third);
        return new BenchmarkRoots<PropertyRoot1, PropertyRoot2, PropertyRoot3>(first, second, third);
    }
}
