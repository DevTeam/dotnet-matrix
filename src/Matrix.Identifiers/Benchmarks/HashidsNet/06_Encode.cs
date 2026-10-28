// ReSharper disable CheckNamespace
namespace Matrix.Identifiers.Benchmarks;

public partial class Encode
{
    private readonly HashidsNet.Hashids _hashidsEncoder = new("matrix", 8);

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.HashidsNet)]
    public string HashidsNet()
    {
        var text = _hashidsEncoder.EncodeLong(IdentifierData.Numbers);
        IdentifierChecks.NotEmpty(LibraryCatalog.HashidsNet, text);
        return text;
    }
}
