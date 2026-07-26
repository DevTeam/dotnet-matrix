using Stashbox;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Combined
{
    private StashboxContainer _stashbox = null!;

    [GlobalSetup(Target = nameof(Stashbox))]
    public void SetupStashbox()
    {
        var container = new StashboxContainer();
        container.RegisterSingleton<ICombinedSingleton, CombinedSingleton>();
        container.Register<ICombinedTransient, CombinedTransient>();
        container.Register<CombinedRoot1>();
        container.Register<CombinedRoot2>();
        container.Register<CombinedRoot3>();
        _stashbox = container;
    }

    [GlobalCleanup(Target = nameof(Stashbox))]
    public void CleanupStashbox() => _stashbox.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Stashbox)]
    public BenchmarkRoots<CombinedRoot1, CombinedRoot2, CombinedRoot3> Stashbox()
    {
        var first = _stashbox.Resolve<CombinedRoot1>();
        var second = _stashbox.Resolve<CombinedRoot2>();
        var third = _stashbox.Resolve<CombinedRoot3>();
        Validate(LibraryCatalog.Stashbox, first, second, third);
        return new(first, second, third);
    }
}
