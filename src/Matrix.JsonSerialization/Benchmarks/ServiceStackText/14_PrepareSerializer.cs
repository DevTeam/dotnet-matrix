// ReSharper disable CheckNamespace
// ReSharper disable UnusedMember.Global
namespace Matrix.JsonSerialization.Benchmarks;

public partial class PrepareSerializer
{
    [LibraryBenchmark(LibraryCatalog.ServiceStackText)]
    [ReportedBenchmark]
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Performance",
        "CA1822:Mark members as static")]
    public void ServiceStackText()
    {
    }
}
