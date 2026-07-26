using LightInject;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Complex
{
    private ServiceContainer _lightInject = null!;

    [GlobalSetup(Target = nameof(LightInject))]
    public void SetupLightInject()
    {
        var container = new ServiceContainer();
        container.Register<IFirstService, FirstService>(new PerContainerLifetime());
        container.Register<ISecondService, SecondService>(new PerContainerLifetime());
        container.Register<IThirdService, ThirdService>(new PerContainerLifetime());
        container.Register<SubObject1>();
        container.Register<SubObject2>();
        container.Register<SubObject3>();
        container.Register<ComplexRoot1>();
        container.Register<ComplexRoot2>();
        container.Register<ComplexRoot3>();
        _lightInject = container;
    }

    [GlobalCleanup(Target = nameof(LightInject))]
    public void CleanupLightInject() => _lightInject.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.LightInject)]
    public BenchmarkRoots<ComplexRoot1, ComplexRoot2, ComplexRoot3> LightInject() =>
        new(
            _lightInject.GetInstance<ComplexRoot1>(),
            _lightInject.GetInstance<ComplexRoot2>(),
            _lightInject.GetInstance<ComplexRoot3>());
}
