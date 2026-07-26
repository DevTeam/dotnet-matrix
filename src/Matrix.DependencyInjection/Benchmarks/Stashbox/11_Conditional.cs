using Stashbox;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Conditional
{
    private StashboxContainer _stashbox = null!;

    [GlobalSetup(Target = nameof(Stashbox))]
    public void SetupStashbox()
    {
        var container = new StashboxContainer();
        container.Register<IConditionalService, ConditionalService1>(
            configurator => configurator.WhenDependantIs<ConditionalRoot1>());
        container.Register<IConditionalService, ConditionalService2>(
            configurator => configurator.WhenDependantIs<ConditionalRoot2>());
        container.Register<IConditionalService, ConditionalService3>(
            configurator => configurator.WhenDependantIs<ConditionalRoot3>());
        container.Register<ConditionalRoot1>();
        container.Register<ConditionalRoot2>();
        container.Register<ConditionalRoot3>();
        _stashbox = container;
    }

    [GlobalCleanup(Target = nameof(Stashbox))]
    public void CleanupStashbox() => _stashbox.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Stashbox)]
    public BenchmarkRoots<ConditionalRoot1, ConditionalRoot2, ConditionalRoot3> Stashbox()
    {
        var first = _stashbox.Resolve<ConditionalRoot1>();
        var second = _stashbox.Resolve<ConditionalRoot2>();
        var third = _stashbox.Resolve<ConditionalRoot3>();
        Validate(LibraryCatalog.Stashbox, first, second, third);
        return new(first, second, third);
    }
}
