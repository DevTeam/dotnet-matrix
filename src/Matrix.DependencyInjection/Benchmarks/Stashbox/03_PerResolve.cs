using Stashbox;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.DependencyInjection.Benchmarks;

public partial class PerResolve
{
    private StashboxContainer _stashbox = null!;

    [GlobalSetup(Target = nameof(Stashbox))]
    public void SetupStashbox()
    {
        var container = new StashboxContainer();
        container.Register<IPerResolveDependency, PerResolveDependency>(
            configurator => configurator.WithPerRequestLifetime());
        container.Register<PerResolveRoot>();
        _stashbox = container;
    }

    [GlobalCleanup(Target = nameof(Stashbox))]
    public void CleanupStashbox() => _stashbox.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Stashbox)]
    public PerResolveRoot Stashbox()
    {
        var root = _stashbox.Resolve<PerResolveRoot>();
        Validate(LibraryCatalog.Stashbox, root);
        return root;
    }
}
