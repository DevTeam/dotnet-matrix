// ReSharper disable CheckNamespace
namespace Matrix.TemplateEngines.Benchmarks;

public partial class Render
{
    private global::Cottle.IDocument _cottleDocument = null!;

    private global::Cottle.IContext _cottleContext = null!;

    [GlobalSetup]
    public void SetupCottle()
    {
        // Cottle substitutes {name}; square brackets are literal text.
        _cottleDocument = global::Cottle.Document
            .CreateDefault("Hello, {name}!", new global::Cottle.DocumentConfiguration())
            .DocumentOrThrow;
        var symbols = new Dictionary<global::Cottle.Value, global::Cottle.Value>
        {
            ["name"] = "World"
        };
        _cottleContext = global::Cottle.Context.CreateBuiltin(symbols);
    }

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Cottle)]
    public string Cottle()
    {
        var text = _cottleDocument.Render(_cottleContext);
        TemplateChecks.Output(LibraryCatalog.Cottle, text, TemplateData.GreetingExpected);
        return text;
    }
}
