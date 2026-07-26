using Grace.DependencyInjection;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Conditional
{
    private DependencyInjectionContainer _grace = null!;

    [GlobalSetup(Target = nameof(Grace))]
    public void SetupGrace()
    {
        var container = new DependencyInjectionContainer();
        container.Configure(block =>
        {
            block.Export<ConditionalService1>()
                .As<IConditionalService>()
                .When.InjectedInto<ConditionalRoot1>();
            block.Export<ConditionalService2>()
                .As<IConditionalService>()
                .When.InjectedInto<ConditionalRoot2>();
            block.Export<ConditionalService3>()
                .As<IConditionalService>()
                .When.InjectedInto<ConditionalRoot3>();
            block.Export<ConditionalRoot1>();
            block.Export<ConditionalRoot2>();
            block.Export<ConditionalRoot3>();
        });
        _grace = container;
    }

    [GlobalCleanup(Target = nameof(Grace))]
    public void CleanupGrace() => _grace.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Grace)]
    public BenchmarkRoots<ConditionalRoot1, ConditionalRoot2, ConditionalRoot3> Grace()
    {
        var first = _grace.Locate<ConditionalRoot1>();
        var second = _grace.Locate<ConditionalRoot2>();
        var third = _grace.Locate<ConditionalRoot3>();
        Validate(LibraryCatalog.Grace, first, second, third);
        return new(first, second, third);
    }
}
