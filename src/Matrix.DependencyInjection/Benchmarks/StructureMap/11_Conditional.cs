using StructureMap;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Conditional
{
    private Container _structureMap = null!;

    [GlobalSetup(Target = nameof(StructureMap))]
    public void SetupStructureMap() =>
        _structureMap = new Container(registry =>
        {
            registry.For<IConditionalService>().AlwaysUnique().Add<ConditionalService1>().Named("1");
            registry.For<IConditionalService>().AlwaysUnique().Add<ConditionalService2>().Named("2");
            registry.For<IConditionalService>().AlwaysUnique().Add<ConditionalService3>().Named("3");
            registry.For<ConditionalRoot1>().AlwaysUnique().Use<ConditionalRoot1>()
                .Ctor<IConditionalService>().IsNamedInstance("1");
            registry.For<ConditionalRoot2>().AlwaysUnique().Use<ConditionalRoot2>()
                .Ctor<IConditionalService>().IsNamedInstance("2");
            registry.For<ConditionalRoot3>().AlwaysUnique().Use<ConditionalRoot3>()
                .Ctor<IConditionalService>().IsNamedInstance("3");
        });

    [GlobalCleanup(Target = nameof(StructureMap))]
    public void CleanupStructureMap() => _structureMap.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.StructureMap)]
    public BenchmarkRoots<ConditionalRoot1, ConditionalRoot2, ConditionalRoot3> StructureMap()
    {
        var first = _structureMap.GetInstance<ConditionalRoot1>();
        var second = _structureMap.GetInstance<ConditionalRoot2>();
        var third = _structureMap.GetInstance<ConditionalRoot3>();
        Validate(LibraryCatalog.StructureMap, first, second, third);
        return new(first, second, third);
    }
}
