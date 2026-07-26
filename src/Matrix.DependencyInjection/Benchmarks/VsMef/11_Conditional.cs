using Microsoft.VisualStudio.Composition;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Conditional
{
    private ExportProvider _vsMef = null!;

    [GlobalSetup(Target = nameof(VsMef))]
    public void SetupVsMef() =>
        _vsMef = VsMefHost.Create(
            typeof(ConditionalService1),
            typeof(ConditionalService2),
            typeof(ConditionalService3),
            typeof(ConditionalRoot1),
            typeof(ConditionalRoot2),
            typeof(ConditionalRoot3));

    [GlobalCleanup(Target = nameof(VsMef))]
    public void CleanupVsMef() => _vsMef.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.VsMef)]
    public BenchmarkRoots<ConditionalRoot1, ConditionalRoot2, ConditionalRoot3> VsMef()
    {
        var first = _vsMef.GetExportedValue<ConditionalRoot1>();
        var second = _vsMef.GetExportedValue<ConditionalRoot2>();
        var third = _vsMef.GetExportedValue<ConditionalRoot3>();
        Validate(LibraryCatalog.VsMef, first, second, third);
        return new(first, second, third);
    }
}
