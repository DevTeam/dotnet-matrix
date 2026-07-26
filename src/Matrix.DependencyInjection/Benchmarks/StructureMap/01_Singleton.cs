using StructureMap;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Singleton
{
    private Container _structureMap = null!;

    [GlobalSetup(Target = nameof(StructureMap))]
    public void SetupStructureMap() =>
        _structureMap = new Container(registry =>
        {
            registry.For<ISingleton1>().Singleton().Use<Singleton1>();
            registry.For<ISingleton2>().Singleton().Use<Singleton2>();
            registry.For<ISingleton3>().Singleton().Use<Singleton3>();
        });

    [GlobalCleanup(Target = nameof(StructureMap))]
    public void CleanupStructureMap() => _structureMap.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.StructureMap)]
    public BenchmarkRoots<ISingleton1, ISingleton2, ISingleton3> StructureMap()
    {
        var first = _structureMap.GetInstance<ISingleton1>();
        var second = _structureMap.GetInstance<ISingleton2>();
        var third = _structureMap.GetInstance<ISingleton3>();
        Validate(LibraryCatalog.StructureMap, first, second, third);
        return new(first, second, third);
    }
}
