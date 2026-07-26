using StructureMap;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Transient
{
    private Container _structureMap = null!;

    [GlobalSetup(Target = nameof(StructureMap))]
    public void SetupStructureMap() =>
        _structureMap = new Container(registry =>
        {
            registry.For<ITransient1>().AlwaysUnique().Use<Transient1>();
            registry.For<ITransient2>().AlwaysUnique().Use<Transient2>();
            registry.For<ITransient3>().AlwaysUnique().Use<Transient3>();
        });

    [GlobalCleanup(Target = nameof(StructureMap))]
    public void CleanupStructureMap() => _structureMap.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.StructureMap)]
    public BenchmarkRoots<ITransient1, ITransient2, ITransient3> StructureMap()
    {
        var first = _structureMap.GetInstance<ITransient1>();
        var second = _structureMap.GetInstance<ITransient2>();
        var third = _structureMap.GetInstance<ITransient3>();
        Validate(LibraryCatalog.StructureMap, first);
        return new(first, second, third);
    }
}
