namespace Matrix.Validation.Benchmarks;

public partial class PrepareValidator
{
    [LibraryBenchmark(LibraryCatalog.DataAnnotations)]
    [ReportedBenchmark]
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Performance",
        "CA1822:Mark members as static")]
    public void DataAnnotations()
    {
    }
}
