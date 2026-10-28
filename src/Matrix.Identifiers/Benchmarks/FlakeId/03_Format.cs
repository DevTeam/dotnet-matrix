// ReSharper disable CheckNamespace
namespace Matrix.Identifiers.Benchmarks;

public partial class Format
{
    private readonly global::FlakeId.Id _flakeValue =
        global::FlakeId.Id.Create(IdentifierData.NextTimestampMilliseconds());

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.FlakeId)]
    public string FlakeId()
    {
        var text = _flakeValue.ToString();
        IdentifierChecks.NotEmpty(LibraryCatalog.FlakeId, text);
        return text;
    }
}
