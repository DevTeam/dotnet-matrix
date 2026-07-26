using Stashbox;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class ChildContainer
{
    private StashboxContainer _stashbox = null!;

    [GlobalSetup(Target = nameof(Stashbox))]
    public void SetupStashbox()
    {
        var container = new StashboxContainer();
        container.Register<IChildValue, ParentValue>();
        container.Register<ChildRoot>();
        _stashbox = container;
    }

    [GlobalCleanup(Target = nameof(Stashbox))]
    public void CleanupStashbox() => _stashbox.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Stashbox)]
    public BenchmarkRoots<ChildRoot, ChildRoot> Stashbox()
    {
        var parent = _stashbox.Resolve<ChildRoot>();
        using var child = _stashbox.CreateChildContainer();
        child.Register<IChildValue, ChildValue>(configurator => configurator.ReplaceExisting());
        var root = child.Resolve<ChildRoot>();
        Validate(LibraryCatalog.Stashbox, parent, root);
        return new(parent, root);
    }
}
