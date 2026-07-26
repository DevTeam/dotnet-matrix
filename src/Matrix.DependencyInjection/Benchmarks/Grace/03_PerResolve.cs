using Grace.DependencyInjection;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.DependencyInjection.Benchmarks;

public partial class PerResolve
{
    private DependencyInjectionContainer _grace = null!;

    [GlobalSetup(Target = nameof(Grace))]
    public void SetupGrace()
    {
        var container = new DependencyInjectionContainer();
        container.Configure(block =>
        {
            block.Export<PerResolveDependency>()
                .As<IPerResolveDependency>()
                .Lifestyle.SingletonPerObjectGraph();
            block.Export<PerResolveRoot>();
        });
        _grace = container;
    }

    [GlobalCleanup(Target = nameof(Grace))]
    public void CleanupGrace() => _grace.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Grace)]
    public PerResolveRoot Grace()
    {
        var root = _grace.Locate<PerResolveRoot>();
        Validate(LibraryCatalog.Grace, root);
        return root;
    }
}
