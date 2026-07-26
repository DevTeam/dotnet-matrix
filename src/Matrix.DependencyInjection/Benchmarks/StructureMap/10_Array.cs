using StructureMap;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Array
{
    private Container _structureMap = null!;

    [GlobalSetup(Target = nameof(StructureMap))]
    public void SetupStructureMap() =>
        _structureMap = new Container(registry =>
        {
            registry.For<IPlugin>().AlwaysUnique().Add<Plugin1>();
            registry.For<IPlugin>().AlwaysUnique().Add<Plugin2>();
            registry.For<IPlugin>().AlwaysUnique().Add<Plugin3>();
            registry.For<IPlugin>().AlwaysUnique().Add<Plugin4>();
            registry.For<IPlugin>().AlwaysUnique().Add<Plugin5>();
            registry.For<ArrayRoot1>().AlwaysUnique().Use<ArrayRoot1>();
            registry.For<ArrayRoot2>().AlwaysUnique().Use<ArrayRoot2>();
            registry.For<ArrayRoot3>().AlwaysUnique().Use<ArrayRoot3>();
        });

    [GlobalCleanup(Target = nameof(StructureMap))]
    public void CleanupStructureMap() => _structureMap.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.StructureMap)]
    public BenchmarkRoots<ArrayRoot1, ArrayRoot2, ArrayRoot3> StructureMap()
    {
        var first = _structureMap.GetInstance<ArrayRoot1>();
        var second = _structureMap.GetInstance<ArrayRoot2>();
        var third = _structureMap.GetInstance<ArrayRoot3>();
        Validate(LibraryCatalog.StructureMap, first, second, third);
        return new(first, second, third);
    }
}
