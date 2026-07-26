using LightInject;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Singleton
{
    private ServiceContainer _lightInject = null!;

    [GlobalSetup(Target = nameof(LightInject))]
    public void SetupLightInject()
    {
        var container = new ServiceContainer();
        container.Register<ISingleton1, Singleton1>(new PerContainerLifetime());
        container.Register<ISingleton2, Singleton2>(new PerContainerLifetime());
        container.Register<ISingleton3, Singleton3>(new PerContainerLifetime());
        _lightInject = container;
    }

    [GlobalCleanup(Target = nameof(LightInject))]
    public void CleanupLightInject() => _lightInject.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.LightInject)]
    public BenchmarkRoots<ISingleton1, ISingleton2, ISingleton3> LightInject()
    {
        var first = _lightInject.GetInstance<ISingleton1>();
        var second = _lightInject.GetInstance<ISingleton2>();
        var third = _lightInject.GetInstance<ISingleton3>();
        Validate(LibraryCatalog.LightInject, first, second, third);
        return new(first, second, third);
    }
}
