using StructureMap;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Property
{
    private Container _structureMap = null!;

    [GlobalSetup(Target = nameof(StructureMap))]
    public void SetupStructureMap() =>
        _structureMap = new Container(registry =>
        {
            registry.Policies.SetAllProperties(convention => convention.OfType<PropertyServiceA>());
            registry.Policies.SetAllProperties(convention => convention.OfType<PropertyServiceB>());
            registry.Policies.SetAllProperties(convention => convention.OfType<PropertyServiceC>());
            registry.For<PropertyServiceA>().AlwaysUnique().Use<PropertyServiceA>();
            registry.For<PropertyServiceB>().AlwaysUnique().Use<PropertyServiceB>();
            registry.For<PropertyServiceC>().AlwaysUnique().Use<PropertyServiceC>();
            registry.For<PropertyRoot1>().AlwaysUnique().Use<PropertyRoot1>();
            registry.For<PropertyRoot2>().AlwaysUnique().Use<PropertyRoot2>();
            registry.For<PropertyRoot3>().AlwaysUnique().Use<PropertyRoot3>();
        });

    [GlobalCleanup(Target = nameof(StructureMap))]
    public void CleanupStructureMap() => _structureMap.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.StructureMap)]
    public BenchmarkRoots<PropertyRoot1, PropertyRoot2, PropertyRoot3> StructureMap()
    {
        var first = _structureMap.GetInstance<PropertyRoot1>();
        var second = _structureMap.GetInstance<PropertyRoot2>();
        var third = _structureMap.GetInstance<PropertyRoot3>();
        Validate(LibraryCatalog.StructureMap, first, second, third);
        return new(first, second, third);
    }
}
