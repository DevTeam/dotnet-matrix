// ReSharper disable CheckNamespace
namespace Matrix.Identifiers.Benchmarks;

public partial class Format
{
    private readonly global::System.Ulid _ulidValue =
        global::System.Ulid.NewUlid(IdentifierData.NextTimestamp());

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Ulid)]
    public string Ulid()
    {
        var text = _ulidValue.ToString();
        IdentifierChecks.TextLength(LibraryCatalog.Ulid, text, IdentifierData.UlidLength);
        return text;
    }
}
