using Stashbox;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Array
{
    private StashboxContainer _stashbox = null!;

    [GlobalSetup(Target = nameof(Stashbox))]
    public void SetupStashbox()
    {
        var container = new StashboxContainer();
        container.Register<IPlugin, Plugin1>();
        container.Register<IPlugin, Plugin2>();
        container.Register<IPlugin, Plugin3>();
        container.Register<IPlugin, Plugin4>();
        container.Register<IPlugin, Plugin5>();
        container.Register<ArrayRoot1>();
        container.Register<ArrayRoot2>();
        container.Register<ArrayRoot3>();
        _stashbox = container;
    }

    [GlobalCleanup(Target = nameof(Stashbox))]
    public void CleanupStashbox() => _stashbox.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Stashbox)]
    public BenchmarkRoots<ArrayRoot1, ArrayRoot2, ArrayRoot3> Stashbox()
    {
        var first = _stashbox.Resolve<ArrayRoot1>();
        var second = _stashbox.Resolve<ArrayRoot2>();
        var third = _stashbox.Resolve<ArrayRoot3>();
        Validate(LibraryCatalog.Stashbox, first, second, third);
        return new(first, second, third);
    }
}
