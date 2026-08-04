// ReSharper disable CheckNamespace
// ReSharper disable UnusedMember.Global
namespace Matrix.Validation.Benchmarks;

public partial class PrepareValidator
{
    [LibraryBenchmark(LibraryCatalog.MiniValidation)]
    [ReportedBenchmark]
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Performance",
        "CA1822:Mark members as static")]
    public void MiniValidation()
    {
    }
}
