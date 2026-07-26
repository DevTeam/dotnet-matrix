using Grace.DependencyInjection;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Scoped
{
    private DependencyInjectionContainer _grace = null!;

    [GlobalSetup(Target = nameof(Grace))]
    public void SetupGrace()
    {
        var container = new DependencyInjectionContainer();
        container.Configure(block =>
        {
            block.Export<ScopedDependency>()
                .As<IScopedDependency>()
                .Lifestyle.SingletonPerScope();
            block.Export<ScopedRoot>();
        });
        _grace = container;
    }

    [GlobalCleanup(Target = nameof(Grace))]
    public void CleanupGrace() => _grace.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Grace)]
    public BenchmarkRoots<ScopedRoot, ScopedRoot> Grace()
    {
        using var scope = _grace.BeginLifetimeScope();
        var first = scope.Locate<ScopedRoot>();
        var second = scope.Locate<ScopedRoot>();
        Validate(LibraryCatalog.Grace, first, second);
        return new(first, second);
    }
}
