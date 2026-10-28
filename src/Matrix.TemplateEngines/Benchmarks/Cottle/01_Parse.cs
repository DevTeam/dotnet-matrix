// ReSharper disable CheckNamespace
namespace Matrix.TemplateEngines.Benchmarks;

public partial class Parse
{
    private const string CottleParseSource = "Hello, {name}!";

    private readonly global::Cottle.DocumentConfiguration _cottleConfiguration = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Cottle)]
    public global::Cottle.IDocument Cottle()
    {
        var document = global::Cottle.Document.CreateDefault(CottleParseSource, _cottleConfiguration).DocumentOrThrow;
        TemplateChecks.NotNull(LibraryCatalog.Cottle, document);
        return document;
    }
}
