using DryIoc;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Scoped
{
    private Container _dryIoc = null!;

    [GlobalSetup(Target = nameof(DryIoc))]
    public void SetupDryIoc()
    {
        var container = new Container();
        container.Register<IScopedDependency, ScopedDependency>(Reuse.Scoped);
        container.Register<ScopedRoot>(Reuse.Transient);
        _dryIoc = container;
    }

    [GlobalCleanup(Target = nameof(DryIoc))]
    public void CleanupDryIoc() => _dryIoc.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.DryIoc)]
    public BenchmarkRoots<ScopedRoot, ScopedRoot> DryIoc()
    {
        using var scope = _dryIoc.OpenScope();
        var first = scope.Resolve<ScopedRoot>();
        var second = scope.Resolve<ScopedRoot>();
        Validate(LibraryCatalog.DryIoc, first, second);
        return new(first, second);
    }
}
