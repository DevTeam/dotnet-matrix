using Microsoft.VisualStudio.Composition;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Transient
{
    private ExportProvider _vsMef = null!;

    [GlobalSetup(Target = nameof(VsMef))]
    public void SetupVsMef() =>
        _vsMef = VsMefHost.Create(typeof(Transient1), typeof(Transient2), typeof(Transient3));

    [GlobalCleanup(Target = nameof(VsMef))]
    public void CleanupVsMef() => _vsMef.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.VsMef)]
    public BenchmarkRoots<ITransient1, ITransient2, ITransient3> VsMef()
    {
        var first = _vsMef.GetExportedValue<ITransient1>();
        var second = _vsMef.GetExportedValue<ITransient2>();
        var third = _vsMef.GetExportedValue<ITransient3>();
        Validate(LibraryCatalog.VsMef, first);
        return new(first, second, third);
    }
}
