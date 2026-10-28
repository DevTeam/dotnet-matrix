// ReSharper disable CheckNamespace
namespace Matrix.Identifiers.Benchmarks;

public partial class Parse
{
    private readonly string _nulidText =
        global::NUlid.Ulid.NewUlid(IdentifierData.NextTimestamp()).ToString();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.NUlid)]
    public global::NUlid.Ulid NUlid()
    {
        var id = global::NUlid.Ulid.Parse(_nulidText);
        IdentifierChecks.Text(LibraryCatalog.NUlid, id.ToString(), _nulidText);
        return id;
    }
}
