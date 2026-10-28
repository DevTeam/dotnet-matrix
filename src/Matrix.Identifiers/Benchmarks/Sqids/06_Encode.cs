// ReSharper disable CheckNamespace
namespace Matrix.Identifiers.Benchmarks;

public partial class Encode
{
    private readonly Sqids.SqidsEncoder<long> _sqidsEncoder = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Sqids)]
    public string Sqids()
    {
        var text = _sqidsEncoder.Encode(IdentifierData.Numbers);
        IdentifierChecks.NotEmpty(LibraryCatalog.Sqids, text);
        return text;
    }
}
