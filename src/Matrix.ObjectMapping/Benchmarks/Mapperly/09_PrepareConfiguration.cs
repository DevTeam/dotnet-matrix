namespace Matrix.ObjectMapping.Benchmarks;

public partial class PrepareConfiguration
{
    [LibraryBenchmark(LibraryCatalog.Mapperly)]
    [ReportedBenchmark]
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Performance",
        "CA1822:Mark members as static")]
    public void Mapperly()
    {
    }
}
