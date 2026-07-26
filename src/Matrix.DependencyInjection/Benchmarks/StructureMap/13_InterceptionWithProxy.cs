using StructureMap;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.DependencyInjection.Benchmarks;

public partial class InterceptionWithProxy
{
    private Container _structureMap = null!;

    [GlobalSetup(Target = nameof(StructureMap))]
    public void SetupStructureMap() =>
        _structureMap = new Container(registry =>
        {
            registry.For<ICalculator>().AlwaysUnique().Use<Calculator>();
            registry.For<ICalculator>().DecorateAllWith(
                "Castle DynamicProxy",
                InterceptionProxy.Create);
        });

    [GlobalCleanup(Target = nameof(StructureMap))]
    public void CleanupStructureMap() => _structureMap.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.StructureMap)]
    public ICalculator StructureMap()
    {
        var calculator = _structureMap.GetInstance<ICalculator>();
        var value = calculator.Add(5, 10);
        Validate(LibraryCatalog.StructureMap, calculator, value);
        return calculator;
    }
}
