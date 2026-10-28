// ReSharper disable CheckNamespace
namespace Matrix.Identifiers.Benchmarks;

public partial class Format
{
    private readonly global::NUlid.Ulid _nulidValue =
        global::NUlid.Ulid.NewUlid(IdentifierData.NextTimestamp());

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.NUlid)]
    public string NUlid()
    {
        var text = _nulidValue.ToString();
        IdentifierChecks.TextLength(LibraryCatalog.NUlid, text, IdentifierData.UlidLength);
        return text;
    }
}
