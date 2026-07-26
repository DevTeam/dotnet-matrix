using DryIoc;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.DependencyInjection.Benchmarks;

public partial class PerResolve
{
    private Container _dryIoc = null!;

    [GlobalSetup(Target = nameof(DryIoc))]
    public void SetupDryIoc()
    {
        var container = new Container();
        container.Register<PerResolveRoot>(setup: Setup.With(openResolutionScope: true));
        container.Register<IPerResolveDependency, PerResolveDependency>(
            Reuse.ScopedToService<PerResolveRoot>());
        _dryIoc = container;
    }

    [GlobalCleanup(Target = nameof(DryIoc))]
    public void CleanupDryIoc() => _dryIoc.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.DryIoc)]
    public PerResolveRoot DryIoc()
    {
        var root = _dryIoc.Resolve<PerResolveRoot>();
        Validate(LibraryCatalog.DryIoc, root);
        return root;
    }
}
