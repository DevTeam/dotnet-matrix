using Microsoft.VisualStudio.Composition;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Singleton
{
    private ExportProvider _vsMef = null!;

    [GlobalSetup(Target = nameof(VsMef))]
    public void SetupVsMef() =>
        _vsMef = VsMefHost.Create(typeof(Singleton1), typeof(Singleton2), typeof(Singleton3));

    [GlobalCleanup(Target = nameof(VsMef))]
    public void CleanupVsMef() => _vsMef.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.VsMef)]
    public BenchmarkRoots<ISingleton1, ISingleton2, ISingleton3> VsMef()
    {
        var first = _vsMef.GetExportedValue<ISingleton1>();
        var second = _vsMef.GetExportedValue<ISingleton2>();
        var third = _vsMef.GetExportedValue<ISingleton3>();
        Validate(LibraryCatalog.VsMef, first, second, third);
        return new(first, second, third);
    }
}
