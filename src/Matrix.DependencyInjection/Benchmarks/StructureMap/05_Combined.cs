using StructureMap;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Combined
{
    private Container _structureMap = null!;

    [GlobalSetup(Target = nameof(StructureMap))]
    public void SetupStructureMap() =>
        _structureMap = new Container(registry =>
        {
            registry.For<ICombinedSingleton>().Singleton().Use<CombinedSingleton>();
            registry.For<ICombinedTransient>().AlwaysUnique().Use<CombinedTransient>();
            registry.For<CombinedRoot1>().AlwaysUnique().Use<CombinedRoot1>();
            registry.For<CombinedRoot2>().AlwaysUnique().Use<CombinedRoot2>();
            registry.For<CombinedRoot3>().AlwaysUnique().Use<CombinedRoot3>();
        });

    [GlobalCleanup(Target = nameof(StructureMap))]
    public void CleanupStructureMap() => _structureMap.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.StructureMap)]
    public BenchmarkRoots<CombinedRoot1, CombinedRoot2, CombinedRoot3> StructureMap()
    {
        var first = _structureMap.GetInstance<CombinedRoot1>();
        var second = _structureMap.GetInstance<CombinedRoot2>();
        var third = _structureMap.GetInstance<CombinedRoot3>();
        Validate(LibraryCatalog.StructureMap, first, second, third);
        return new(first, second, third);
    }
}
