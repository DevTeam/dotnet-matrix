// ReSharper disable CheckNamespace
namespace Matrix.Identifiers.Benchmarks;

public partial class Decode
{
    private readonly HashidsNet.Hashids _hashidsDecoder = new("matrix", 8);

    private readonly string _hashidsText = new HashidsNet.Hashids("matrix", 8).EncodeLong(IdentifierData.Numbers);

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.HashidsNet)]
    public long[] HashidsNet()
    {
        var numbers = _hashidsDecoder.DecodeLong(_hashidsText);
        IdentifierChecks.Numbers(LibraryCatalog.HashidsNet, numbers, IdentifierData.Numbers);
        return numbers;
    }
}
