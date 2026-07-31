namespace Matrix.Validation.Benchmarks;

public partial class PrepareValidator
{
    [LibraryBenchmark(LibraryCatalog.MicrosoftExtensionsValidation)]
    [ReportedBenchmark]
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Performance",
        "CA1822:Mark members as static")]
    public void MicrosoftExtensionsValidation()
    {
    }
}
