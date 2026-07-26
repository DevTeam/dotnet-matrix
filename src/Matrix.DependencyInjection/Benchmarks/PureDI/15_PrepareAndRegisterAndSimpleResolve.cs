// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.DependencyInjection.Benchmarks;

public partial class PrepareAndRegisterAndSimpleResolve
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.PureDi)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public ISingleton1 PureDI()
    {
        var composition = new PureDiPrepareComposition();
        return composition.Root;
    }
}
