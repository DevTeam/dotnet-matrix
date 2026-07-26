using LightInject;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Conditional
{
    private ServiceContainer _lightInject = null!;

    [GlobalSetup(Target = nameof(LightInject))]
    public void SetupLightInject()
    {
        var container = new ServiceContainer();
        container.Register<IConditionalService, ConditionalService1>("1");
        container.Register<IConditionalService, ConditionalService2>("2");
        container.Register<IConditionalService, ConditionalService3>("3");
        container.Register<ConditionalRoot1>();
        container.Register<ConditionalRoot2>();
        container.Register<ConditionalRoot3>();
        container.RegisterConstructorDependency<IConditionalService>((factory, parameter) =>
            factory.GetInstance<IConditionalService>(
                parameter.Member.DeclaringType == typeof(ConditionalRoot1) ? "1"
                : parameter.Member.DeclaringType == typeof(ConditionalRoot2) ? "2"
                : "3"));
        _lightInject = container;
    }

    [GlobalCleanup(Target = nameof(LightInject))]
    public void CleanupLightInject() => _lightInject.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.LightInject)]
    public BenchmarkRoots<ConditionalRoot1, ConditionalRoot2, ConditionalRoot3> LightInject()
    {
        var first = _lightInject.GetInstance<ConditionalRoot1>();
        var second = _lightInject.GetInstance<ConditionalRoot2>();
        var third = _lightInject.GetInstance<ConditionalRoot3>();
        Validate(LibraryCatalog.LightInject, first, second, third);
        return new(first, second, third);
    }
}
