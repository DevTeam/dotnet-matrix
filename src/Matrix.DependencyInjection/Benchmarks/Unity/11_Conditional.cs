using Unity;
using Unity.Injection;
using Unity.Lifetime;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Conditional
{
    private IUnityContainer _unity = null!;

    [GlobalSetup(Target = nameof(Unity))]
    public void SetupUnity()
    {
        var container = new UnityContainer();
        container.RegisterType<IConditionalService, ConditionalService1>("1", new TransientLifetimeManager());
        container.RegisterType<IConditionalService, ConditionalService2>("2", new TransientLifetimeManager());
        container.RegisterType<IConditionalService, ConditionalService3>("3", new TransientLifetimeManager());
        container.RegisterType<ConditionalRoot1>(
            new TransientLifetimeManager(),
            new InjectionConstructor(new ResolvedParameter<IConditionalService>("1")));
        container.RegisterType<ConditionalRoot2>(
            new TransientLifetimeManager(),
            new InjectionConstructor(new ResolvedParameter<IConditionalService>("2")));
        container.RegisterType<ConditionalRoot3>(
            new TransientLifetimeManager(),
            new InjectionConstructor(new ResolvedParameter<IConditionalService>("3")));
        _unity = container;
    }

    [GlobalCleanup(Target = nameof(Unity))]
    public void CleanupUnity() => _unity.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Unity)]
    public BenchmarkRoots<ConditionalRoot1, ConditionalRoot2, ConditionalRoot3> Unity()
    {
        var first = _unity.Resolve<ConditionalRoot1>();
        var second = _unity.Resolve<ConditionalRoot2>();
        var third = _unity.Resolve<ConditionalRoot3>();
        Validate(LibraryCatalog.Unity, first, second, third);
        return new(first, second, third);
    }
}
