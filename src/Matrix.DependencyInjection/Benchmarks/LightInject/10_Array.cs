using LightInject;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Array
{
    private ServiceContainer _lightInject = null!;

    [GlobalSetup(Target = nameof(LightInject))]
    public void SetupLightInject()
    {
        var container = new ServiceContainer();
        container.Register<IPlugin, Plugin1>("1");
        container.Register<IPlugin, Plugin2>("2");
        container.Register<IPlugin, Plugin3>("3");
        container.Register<IPlugin, Plugin4>("4");
        container.Register<IPlugin, Plugin5>("5");
        container.Register<ArrayRoot1>();
        container.Register<ArrayRoot2>();
        container.Register<ArrayRoot3>();
        _lightInject = container;
    }

    [GlobalCleanup(Target = nameof(LightInject))]
    public void CleanupLightInject() => _lightInject.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.LightInject)]
    public BenchmarkRoots<ArrayRoot1, ArrayRoot2, ArrayRoot3> LightInject()
    {
        var first = _lightInject.GetInstance<ArrayRoot1>();
        var second = _lightInject.GetInstance<ArrayRoot2>();
        var third = _lightInject.GetInstance<ArrayRoot3>();
        Validate(LibraryCatalog.LightInject, first, second, third);
        return new(first, second, third);
    }
}
