using DryIoc;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Property
{
    private Container _dryIoc = null!;

    [GlobalSetup(Target = nameof(DryIoc))]
    public void SetupDryIoc()
    {
        var container = new Container(rules =>
            rules.With(propertiesAndFields: PropertiesAndFields.Auto));
        container.Register<PropertyServiceA>(Reuse.Transient);
        container.Register<PropertyServiceB>(Reuse.Transient);
        container.Register<PropertyServiceC>(Reuse.Transient);
        container.Register<PropertyRoot1>(Reuse.Transient);
        container.Register<PropertyRoot2>(Reuse.Transient);
        container.Register<PropertyRoot3>(Reuse.Transient);
        _dryIoc = container;
    }

    [GlobalCleanup(Target = nameof(DryIoc))]
    public void CleanupDryIoc() => _dryIoc.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.DryIoc)]
    public BenchmarkRoots<PropertyRoot1, PropertyRoot2, PropertyRoot3> DryIoc()
    {
        var first = _dryIoc.Resolve<PropertyRoot1>();
        var second = _dryIoc.Resolve<PropertyRoot2>();
        var third = _dryIoc.Resolve<PropertyRoot3>();
        Validate(LibraryCatalog.DryIoc, first, second, third);
        return new(first, second, third);
    }
}
