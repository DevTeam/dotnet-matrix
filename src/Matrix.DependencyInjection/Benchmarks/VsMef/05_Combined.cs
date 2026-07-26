using Microsoft.VisualStudio.Composition;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Combined
{
    private ExportProvider _vsMef = null!;

    [GlobalSetup(Target = nameof(VsMef))]
    public void SetupVsMef() =>
        _vsMef = VsMefHost.Create(
            typeof(CombinedSingleton),
            typeof(CombinedTransient),
            typeof(CombinedRoot1),
            typeof(CombinedRoot2),
            typeof(CombinedRoot3));

    [GlobalCleanup(Target = nameof(VsMef))]
    public void CleanupVsMef() => _vsMef.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.VsMef)]
    public BenchmarkRoots<CombinedRoot1, CombinedRoot2, CombinedRoot3> VsMef()
    {
        var first = _vsMef.GetExportedValue<CombinedRoot1>();
        var second = _vsMef.GetExportedValue<CombinedRoot2>();
        var third = _vsMef.GetExportedValue<CombinedRoot3>();
        Validate(LibraryCatalog.VsMef, first, second, third);
        return new(first, second, third);
    }
}
