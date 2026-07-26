using LightInject;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Property
{
    private ServiceContainer _lightInject = null!;

    [GlobalSetup(Target = nameof(LightInject))]
    public void SetupLightInject()
    {
        var container = new ServiceContainer(new ContainerOptions { EnablePropertyInjection = true });
        container.Register<PropertyServiceA>();
        container.Register<PropertyServiceB>();
        container.Register<PropertyServiceC>();
        container.Register<PropertyRoot1>();
        container.Register<PropertyRoot2>();
        container.Register<PropertyRoot3>();
        _lightInject = container;
    }

    [GlobalCleanup(Target = nameof(LightInject))]
    public void CleanupLightInject() => _lightInject.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.LightInject)]
    public BenchmarkRoots<PropertyRoot1, PropertyRoot2, PropertyRoot3> LightInject()
    {
        var first = _lightInject.GetInstance<PropertyRoot1>();
        var second = _lightInject.GetInstance<PropertyRoot2>();
        var third = _lightInject.GetInstance<PropertyRoot3>();
        Validate(LibraryCatalog.LightInject, first, second, third);
        return new(first, second, third);
    }
}
