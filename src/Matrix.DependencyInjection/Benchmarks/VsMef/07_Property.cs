using Microsoft.VisualStudio.Composition;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Property
{
    private ExportProvider _vsMef = null!;

    [GlobalSetup(Target = nameof(VsMef))]
    public void SetupVsMef() =>
        _vsMef = VsMefHost.Create(
            typeof(PropertyServiceA),
            typeof(PropertyServiceB),
            typeof(PropertyServiceC),
            typeof(PropertyRoot1),
            typeof(PropertyRoot2),
            typeof(PropertyRoot3));

    [GlobalCleanup(Target = nameof(VsMef))]
    public void CleanupVsMef() => _vsMef.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.VsMef)]
    public BenchmarkRoots<PropertyRoot1, PropertyRoot2, PropertyRoot3> VsMef()
    {
        var first = _vsMef.GetExportedValue<PropertyRoot1>();
        var second = _vsMef.GetExportedValue<PropertyRoot2>();
        var third = _vsMef.GetExportedValue<PropertyRoot3>();
        Validate(LibraryCatalog.VsMef, first, second, third);
        return new(first, second, third);
    }
}
