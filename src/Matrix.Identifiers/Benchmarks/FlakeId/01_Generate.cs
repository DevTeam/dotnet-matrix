// ReSharper disable CheckNamespace
namespace Matrix.Identifiers.Benchmarks;

public partial class Generate
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.FlakeId)]
    public global::FlakeId.Id FlakeId()
    {
        var id = global::FlakeId.Id.Create(IdentifierData.NextTimestampMilliseconds());
        IdentifierChecks.NotEmpty(LibraryCatalog.FlakeId, id.ToString());
        return id;
    }
}
