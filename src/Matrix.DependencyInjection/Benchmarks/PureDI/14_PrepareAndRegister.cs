// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
namespace Matrix.DependencyInjection.Benchmarks;

public partial class PrepareAndRegister
{
    [LibraryBenchmark(LibraryCatalog.PureDi)]
    [ReportedBenchmark]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public void PureDI()
    {
    }
}