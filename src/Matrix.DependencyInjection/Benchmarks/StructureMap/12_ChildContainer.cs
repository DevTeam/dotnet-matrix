using StructureMap;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class ChildContainer
{
    private Container _structureMap = null!;

    [GlobalSetup(Target = nameof(StructureMap))]
    public void SetupStructureMap() =>
        _structureMap = new Container(registry =>
        {
            registry.For<IChildValue>().AlwaysUnique().Use<ParentValue>();
            registry.For<ChildRoot>().AlwaysUnique().Use<ChildRoot>();
        });

    [GlobalCleanup(Target = nameof(StructureMap))]
    public void CleanupStructureMap() => _structureMap.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.StructureMap)]
    public BenchmarkRoots<ChildRoot, ChildRoot> StructureMap()
    {
        var parent = _structureMap.GetInstance<ChildRoot>();
        using var child = _structureMap.CreateChildContainer();
        child.Configure(registry => registry.For<IChildValue>().AlwaysUnique().Use<ChildValue>());
        var root = child.GetInstance<ChildRoot>();
        Validate(LibraryCatalog.StructureMap, parent, root);
        return new(parent, root);
    }
}
