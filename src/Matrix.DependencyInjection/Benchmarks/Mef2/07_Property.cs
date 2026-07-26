using System.Composition.Hosting;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Property
{
    private CompositionHost _mef2 = null!;

    [GlobalSetup(Target = nameof(Mef2))]
    public void SetupMef2() =>
        _mef2 = new ContainerConfiguration()
            .WithParts(
                typeof(PropertyServiceA),
                typeof(PropertyServiceB),
                typeof(PropertyServiceC),
                typeof(PropertyRoot1),
                typeof(PropertyRoot2),
                typeof(PropertyRoot3))
            .CreateContainer();

    [GlobalCleanup(Target = nameof(Mef2))]
    public void CleanupMef2() => _mef2.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Mef2)]
    public BenchmarkRoots<PropertyRoot1, PropertyRoot2, PropertyRoot3> Mef2()
    {
        var first = _mef2.GetExport<PropertyRoot1>();
        var second = _mef2.GetExport<PropertyRoot2>();
        var third = _mef2.GetExport<PropertyRoot3>();
        Validate(LibraryCatalog.Mef2, first, second, third);
        return new(first, second, third);
    }
}
