using Grace.DependencyInjection;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Property
{
    private DependencyInjectionContainer _grace = null!;

    [GlobalSetup(Target = nameof(Grace))]
    public void SetupGrace()
    {
        var container = new DependencyInjectionContainer();
        container.Configure(block =>
        {
            block.Export<PropertyServiceA>();
            block.Export<PropertyServiceB>();
            block.Export<PropertyServiceC>();
            block.Export<PropertyRoot1>().AutoWireProperties();
            block.Export<PropertyRoot2>().AutoWireProperties();
            block.Export<PropertyRoot3>().AutoWireProperties();
        });
        _grace = container;
    }

    [GlobalCleanup(Target = nameof(Grace))]
    public void CleanupGrace() => _grace.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Grace)]
    public BenchmarkRoots<PropertyRoot1, PropertyRoot2, PropertyRoot3> Grace()
    {
        var first = _grace.Locate<PropertyRoot1>();
        var second = _grace.Locate<PropertyRoot2>();
        var third = _grace.Locate<PropertyRoot3>();
        Validate(LibraryCatalog.Grace, first, second, third);
        return new(first, second, third);
    }
}
