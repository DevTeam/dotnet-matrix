// ReSharper disable CheckNamespace
namespace Matrix.Identifiers.Benchmarks;

public partial class Format
{
    private readonly global::Medo.Uuid7 _medoValue =
        global::Medo.Uuid7.NewUuid7(IdentifierData.NextTimestamp());

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.MedoUuid7)]
    public string MedoUuid7()
    {
        var text = _medoValue.ToString();
        IdentifierChecks.TextLength(LibraryCatalog.MedoUuid7, text, IdentifierData.GuidLength);
        return text;
    }
}
