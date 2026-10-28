// ReSharper disable CheckNamespace
namespace Matrix.Identifiers.Benchmarks;

public partial class Decode
{
    private readonly Sqids.SqidsEncoder<long> _sqidsDecoder = new();

    private readonly string _sqidsText = new Sqids.SqidsEncoder<long>().Encode(IdentifierData.Numbers);

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Sqids)]
    public long[] Sqids()
    {
        var numbers = _sqidsDecoder.Decode(_sqidsText);
        IdentifierChecks.Numbers(LibraryCatalog.Sqids, numbers, IdentifierData.Numbers);
        return [.. numbers];
    }
}
