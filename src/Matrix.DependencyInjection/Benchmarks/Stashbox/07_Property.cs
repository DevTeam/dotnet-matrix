using Stashbox;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Property
{
    private StashboxContainer _stashbox = null!;

    [GlobalSetup(Target = nameof(Stashbox))]
    public void SetupStashbox()
    {
        var container = new StashboxContainer(configurator => configurator.WithAutoMemberInjection());
        container.Register<PropertyServiceA>();
        container.Register<PropertyServiceB>();
        container.Register<PropertyServiceC>();
        container.Register<PropertyRoot1>();
        container.Register<PropertyRoot2>();
        container.Register<PropertyRoot3>();
        _stashbox = container;
    }

    [GlobalCleanup(Target = nameof(Stashbox))]
    public void CleanupStashbox() => _stashbox.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Stashbox)]
    public BenchmarkRoots<PropertyRoot1, PropertyRoot2, PropertyRoot3> Stashbox()
    {
        var first = _stashbox.Resolve<PropertyRoot1>();
        var second = _stashbox.Resolve<PropertyRoot2>();
        var third = _stashbox.Resolve<PropertyRoot3>();
        Validate(LibraryCatalog.Stashbox, first, second, third);
        return new(first, second, third);
    }
}
