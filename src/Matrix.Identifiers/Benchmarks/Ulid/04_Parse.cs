// ReSharper disable CheckNamespace
namespace Matrix.Identifiers.Benchmarks;

public partial class Parse
{
    private readonly string _ulidText =
        global::System.Ulid.NewUlid(IdentifierData.NextTimestamp()).ToString();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Ulid)]
    public global::System.Ulid Ulid()
    {
        var id = global::System.Ulid.Parse(_ulidText);
        IdentifierChecks.Text(LibraryCatalog.Ulid, id.ToString(), _ulidText);
        return id;
    }
}
