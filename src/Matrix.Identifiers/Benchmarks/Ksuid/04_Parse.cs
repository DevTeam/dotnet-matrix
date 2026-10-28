// ReSharper disable CheckNamespace
namespace Matrix.Identifiers.Benchmarks;

public partial class Parse
{
    private readonly string _ksuidText = global::KSUID.Ksuid.Generate().ToString();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Ksuid)]
    public global::KSUID.Ksuid Ksuid()
    {
        var id = global::KSUID.Ksuid.FromString(_ksuidText);
        IdentifierChecks.Text(LibraryCatalog.Ksuid, id.ToString(), _ksuidText);
        return id;
    }
}
