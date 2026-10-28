// ReSharper disable CheckNamespace
namespace Matrix.Identifiers.Benchmarks;

public partial class Parse
{
    private readonly long _flakeNumber =
        (long)global::FlakeId.Id.Create(IdentifierData.NextTimestampMilliseconds());

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.FlakeId)]
    public global::FlakeId.Id FlakeId()
    {
        var id = global::FlakeId.Id.Parse(_flakeNumber);
        IdentifierChecks.Reflects(LibraryCatalog.FlakeId, (long)id == _flakeNumber, "The parsed identifier");
        return id;
    }
}
