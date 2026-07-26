using StructureMap;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Scoped
{
    private Container _structureMap = null!;

    [GlobalSetup(Target = nameof(StructureMap))]
    public void SetupStructureMap() =>
        _structureMap = new Container(registry =>
        {
            registry.For<IScopedDependency>().ContainerScoped().Use<ScopedDependency>();
            registry.For<ScopedRoot>().AlwaysUnique().Use<ScopedRoot>();
        });

    [GlobalCleanup(Target = nameof(StructureMap))]
    public void CleanupStructureMap() => _structureMap.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.StructureMap)]
    public BenchmarkRoots<ScopedRoot, ScopedRoot> StructureMap()
    {
        using var scope = _structureMap.GetNestedContainer();
        var first = scope.GetInstance<ScopedRoot>();
        var second = scope.GetInstance<ScopedRoot>();
        Validate(LibraryCatalog.StructureMap, first, second);
        return new(first, second);
    }
}
