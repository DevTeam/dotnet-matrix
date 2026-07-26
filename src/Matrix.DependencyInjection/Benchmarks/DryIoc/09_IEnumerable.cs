using DryIoc;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Enumerable
{
    private Container _dryIoc = null!;

    [GlobalSetup(Target = nameof(DryIoc))]
    public void SetupDryIoc()
    {
        var container = new Container(rules => rules.WithResolveIEnumerableAsLazyEnumerable());
        container.Register<IPlugin, Plugin1>(Reuse.Transient);
        container.Register<IPlugin, Plugin2>(Reuse.Transient);
        container.Register<IPlugin, Plugin3>(Reuse.Transient);
        container.Register<IPlugin, Plugin4>(Reuse.Transient);
        container.Register<IPlugin, Plugin5>(Reuse.Transient);
        container.Register<EnumerableRoot1>(Reuse.Transient);
        container.Register<EnumerableRoot2>(Reuse.Transient);
        container.Register<EnumerableRoot3>(Reuse.Transient);
        _dryIoc = container;
    }

    [GlobalCleanup(Target = nameof(DryIoc))]
    public void CleanupDryIoc() => _dryIoc.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.DryIoc)]
    public BenchmarkRoots<EnumerableRoot1, EnumerableRoot2, EnumerableRoot3> DryIoc()
    {
        var first = _dryIoc.Resolve<EnumerableRoot1>();
        var second = _dryIoc.Resolve<EnumerableRoot2>();
        var third = _dryIoc.Resolve<EnumerableRoot3>();
        Validate(LibraryCatalog.DryIoc, first, second, third);
        return new(first, second, third);
    }
}
