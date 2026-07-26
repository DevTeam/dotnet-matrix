using DryIoc;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class ChildContainer
{
    private Container _dryIoc = null!;

    [GlobalSetup(Target = nameof(DryIoc))]
    public void SetupDryIoc()
    {
        var container = new Container();
        container.Register<IChildValue, ParentValue>(Reuse.Transient);
        container.Register<ChildRoot>(Reuse.Transient);
        _dryIoc = container;
    }

    [GlobalCleanup(Target = nameof(DryIoc))]
    public void CleanupDryIoc() => _dryIoc.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.DryIoc)]
    public BenchmarkRoots<ChildRoot, ChildRoot> DryIoc()
    {
        var parent = _dryIoc.Resolve<ChildRoot>();
        using var child = _dryIoc.CreateChild(IfAlreadyRegistered.Replace);
        child.Register<IChildValue, ChildValue>(Reuse.Transient);
        var root = child.Resolve<ChildRoot>();
        Validate(LibraryCatalog.DryIoc, parent, root);
        return new(parent, root);
    }
}
