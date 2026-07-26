using Stashbox;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Scoped
{
    private StashboxContainer _stashbox = null!;

    [GlobalSetup(Target = nameof(Stashbox))]
    public void SetupStashbox()
    {
        var container = new StashboxContainer();
        container.RegisterScoped<IScopedDependency, ScopedDependency>();
        container.Register<ScopedRoot>();
        _stashbox = container;
    }

    [GlobalCleanup(Target = nameof(Stashbox))]
    public void CleanupStashbox() => _stashbox.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Stashbox)]
    public BenchmarkRoots<ScopedRoot, ScopedRoot> Stashbox()
    {
        using var scope = _stashbox.BeginScope();
        var first = scope.Resolve<ScopedRoot>();
        var second = scope.Resolve<ScopedRoot>();
        Validate(LibraryCatalog.Stashbox, first, second);
        return new(first, second);
    }
}
