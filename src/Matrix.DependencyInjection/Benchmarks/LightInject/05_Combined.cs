using LightInject;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Combined
{
    private ServiceContainer _lightInject = null!;

    [GlobalSetup(Target = nameof(LightInject))]
    public void SetupLightInject()
    {
        var container = new ServiceContainer();
        container.Register<ICombinedSingleton, CombinedSingleton>(new PerContainerLifetime());
        container.Register<ICombinedTransient, CombinedTransient>();
        container.Register<CombinedRoot1>();
        container.Register<CombinedRoot2>();
        container.Register<CombinedRoot3>();
        _lightInject = container;
    }

    [GlobalCleanup(Target = nameof(LightInject))]
    public void CleanupLightInject() => _lightInject.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.LightInject)]
    public BenchmarkRoots<CombinedRoot1, CombinedRoot2, CombinedRoot3> LightInject()
    {
        var first = _lightInject.GetInstance<CombinedRoot1>();
        var second = _lightInject.GetInstance<CombinedRoot2>();
        var third = _lightInject.GetInstance<CombinedRoot3>();
        Validate(LibraryCatalog.LightInject, first, second, third);
        return new BenchmarkRoots<CombinedRoot1, CombinedRoot2, CombinedRoot3>(first, second, third);
    }
}
