// ReSharper disable CheckNamespace
namespace Matrix.Identifiers.Benchmarks;

public partial class Format
{
    private readonly Visus.Cuid.Cuid2 _cuidNetValue = new(IdentifierData.RandomLength);

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.CuidNet)]
    public string CuidNet()
    {
        var text = _cuidNetValue.ToString();
        IdentifierChecks.TextLength(LibraryCatalog.CuidNet, text, IdentifierData.RandomLength);
        return text;
    }
}
