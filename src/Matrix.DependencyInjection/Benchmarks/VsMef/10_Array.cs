using Microsoft.VisualStudio.Composition;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Array
{
    private ExportProvider _vsMef = null!;

    [GlobalSetup(Target = nameof(VsMef))]
    public void SetupVsMef() =>
        _vsMef = VsMefHost.Create(
            typeof(Plugin1),
            typeof(Plugin2),
            typeof(Plugin3),
            typeof(Plugin4),
            typeof(Plugin5),
            typeof(ArrayRoot1),
            typeof(ArrayRoot2),
            typeof(ArrayRoot3));

    [GlobalCleanup(Target = nameof(VsMef))]
    public void CleanupVsMef() => _vsMef.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.VsMef)]
    public BenchmarkRoots<ArrayRoot1, ArrayRoot2, ArrayRoot3> VsMef()
    {
        var first = _vsMef.GetExportedValue<ArrayRoot1>();
        var second = _vsMef.GetExportedValue<ArrayRoot2>();
        var third = _vsMef.GetExportedValue<ArrayRoot3>();
        Validate(LibraryCatalog.VsMef, first, second, third);
        return new(first, second, third);
    }
}
