// ReSharper disable CheckNamespace
namespace Matrix.Identifiers.Benchmarks;

public partial class Format
{
    private readonly global::KSUID.Ksuid _ksuidValue =
        new(new byte[16], IdentifierData.KsuidTimestamp);

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Ksuid)]
    public string Ksuid()
    {
        var text = _ksuidValue.ToString();
        IdentifierChecks.TextLength(LibraryCatalog.Ksuid, text, IdentifierData.KsuidLength);
        return text;
    }
}
