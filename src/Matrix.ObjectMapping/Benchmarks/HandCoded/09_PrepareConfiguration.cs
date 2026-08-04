// ReSharper disable CheckNamespace
// ReSharper disable UnusedMember.Global
namespace Matrix.ObjectMapping.Benchmarks;

public partial class PrepareConfiguration
{
    [LibraryBenchmark(LibraryCatalog.HandCoded)]
    [ReportedBenchmark]
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Performance",
        "CA1822:Mark members as static")]
    public void HandCoded()
    {
    }
}
