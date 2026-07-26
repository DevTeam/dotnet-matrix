using Stashbox;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Transient
{
    private StashboxContainer _stashbox = null!;

    [GlobalSetup(Target = nameof(Stashbox))]
    public void SetupStashbox()
    {
        var container = new StashboxContainer();
        container.Register<ITransient1, Transient1>();
        container.Register<ITransient2, Transient2>();
        container.Register<ITransient3, Transient3>();
        _stashbox = container;
    }

    [GlobalCleanup(Target = nameof(Stashbox))]
    public void CleanupStashbox() => _stashbox.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Stashbox)]
    public BenchmarkRoots<ITransient1, ITransient2, ITransient3> Stashbox()
    {
        var first = _stashbox.Resolve<ITransient1>();
        var second = _stashbox.Resolve<ITransient2>();
        var third = _stashbox.Resolve<ITransient3>();
        Validate(LibraryCatalog.Stashbox, first);
        return new(first, second, third);
    }
}
