// ReSharper disable CheckNamespace
namespace Matrix.Identifiers.Benchmarks;

public partial class Parse
{
    private readonly string _medoText =
        global::Medo.Uuid7.NewUuid7(IdentifierData.NextTimestamp()).ToString();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.MedoUuid7)]
    public global::Medo.Uuid7 MedoUuid7()
    {
        var id = global::Medo.Uuid7.Parse(_medoText);
        IdentifierChecks.Text(LibraryCatalog.MedoUuid7, id.ToString(), _medoText);
        return id;
    }
}
