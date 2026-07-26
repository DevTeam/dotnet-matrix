using Lamar;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Conditional
{
    private Container _lamar = null!;

    [GlobalSetup(Target = nameof(Lamar))]
    public void SetupLamar() =>
        _lamar = Container.For(registry =>
        {
            registry.For<IConditionalService>().Add<ConditionalService1>().Named("1").Transient();
            registry.For<IConditionalService>().Add<ConditionalService2>().Named("2").Transient();
            registry.For<IConditionalService>().Add<ConditionalService3>().Named("3").Transient();
            registry.For<ConditionalRoot1>().Use<ConditionalRoot1>()
                .Ctor<IConditionalService>().IsNamedInstance("1");
            registry.For<ConditionalRoot2>().Use<ConditionalRoot2>()
                .Ctor<IConditionalService>().IsNamedInstance("2");
            registry.For<ConditionalRoot3>().Use<ConditionalRoot3>()
                .Ctor<IConditionalService>().IsNamedInstance("3");
        });

    [GlobalCleanup(Target = nameof(Lamar))]
    public void CleanupLamar() => _lamar.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Lamar)]
    public BenchmarkRoots<ConditionalRoot1, ConditionalRoot2, ConditionalRoot3> Lamar()
    {
        var first = _lamar.GetInstance<ConditionalRoot1>();
        var second = _lamar.GetInstance<ConditionalRoot2>();
        var third = _lamar.GetInstance<ConditionalRoot3>();
        Validate(LibraryCatalog.Lamar, first, second, third);
        return new(first, second, third);
    }
}
