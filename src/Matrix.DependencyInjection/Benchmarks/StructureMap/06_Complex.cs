using StructureMap;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Complex
{
    private Container _structureMap = null!;

    [GlobalSetup(Target = nameof(StructureMap))]
    public void SetupStructureMap() =>
        _structureMap = new Container(registry =>
        {
            registry.For<IFirstService>().Singleton().Use<FirstService>();
            registry.For<ISecondService>().Singleton().Use<SecondService>();
            registry.For<IThirdService>().Singleton().Use<ThirdService>();
            registry.For<SubObject1>().AlwaysUnique().Use<SubObject1>();
            registry.For<SubObject2>().AlwaysUnique().Use<SubObject2>();
            registry.For<SubObject3>().AlwaysUnique().Use<SubObject3>();
            registry.For<ComplexRoot1>().AlwaysUnique().Use<ComplexRoot1>();
            registry.For<ComplexRoot2>().AlwaysUnique().Use<ComplexRoot2>();
            registry.For<ComplexRoot3>().AlwaysUnique().Use<ComplexRoot3>();
        });

    [GlobalCleanup(Target = nameof(StructureMap))]
    public void CleanupStructureMap() => _structureMap.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.StructureMap)]
    public BenchmarkRoots<ComplexRoot1, ComplexRoot2, ComplexRoot3> StructureMap() =>
        new(
            _structureMap.GetInstance<ComplexRoot1>(),
            _structureMap.GetInstance<ComplexRoot2>(),
            _structureMap.GetInstance<ComplexRoot3>());
}
