// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.DependencyInjection.Benchmarks;

public partial class PrepareAndRegister
{
    [LibraryBenchmark(LibraryCatalog.HandCoded)]
    [ReportedBenchmark]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public void HandCoded()
    {
    }
}
