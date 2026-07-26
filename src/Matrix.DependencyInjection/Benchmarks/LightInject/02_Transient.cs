using LightInject;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Transient
{
    private ServiceContainer _lightInject = null!;

    [GlobalSetup(Target = nameof(LightInject))]
    public void SetupLightInject()
    {
        var container = new ServiceContainer();
        container.Register<ITransient1, Transient1>();
        container.Register<ITransient2, Transient2>();
        container.Register<ITransient3, Transient3>();
        _lightInject = container;
    }

    [GlobalCleanup(Target = nameof(LightInject))]
    public void CleanupLightInject() => _lightInject.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.LightInject)]
    public BenchmarkRoots<ITransient1, ITransient2, ITransient3> LightInject()
    {
        var first = _lightInject.GetInstance<ITransient1>();
        var second = _lightInject.GetInstance<ITransient2>();
        var third = _lightInject.GetInstance<ITransient3>();
        Validate(LibraryCatalog.LightInject, first);
        return new(first, second, third);
    }
}
