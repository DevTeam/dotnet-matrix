using StructureMap;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Generics
{
    private Container _structureMap = null!;

    [GlobalSetup(Target = nameof(StructureMap))]
    public void SetupStructureMap() =>
        _structureMap = new Container(registry =>
        {
            registry.For(typeof(IGenericService<>)).Use(typeof(GenericService<>));
            registry.For(typeof(GenericRoot<>)).Use(typeof(GenericRoot<>));
        });

    [GlobalCleanup(Target = nameof(StructureMap))]
    public void CleanupStructureMap() => _structureMap.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.StructureMap)]
    public BenchmarkRoots<GenericRoot<int>, GenericRoot<float>, GenericRoot<object>> StructureMap() =>
        new(
            _structureMap.GetInstance<GenericRoot<int>>(),
            _structureMap.GetInstance<GenericRoot<float>>(),
            _structureMap.GetInstance<GenericRoot<object>>());
}
