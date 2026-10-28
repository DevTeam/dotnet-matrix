// ReSharper disable CheckNamespace
namespace Matrix.TemplateEngines.Benchmarks;

public partial class Escape
{
    private global::Stubble.Core.StubbleVisitorRenderer _stubbleEscapeRenderer = null!;

    private object _stubbleEscapeModel = null!;

    [GlobalSetup]
    public void SetupStubble()
    {
        _stubbleEscapeRenderer = new global::Stubble.Core.Builders.StubbleBuilder().Build();
        _stubbleEscapeModel = new { name = TemplateData.RawUnsafe };
    }

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Stubble)]
    public string Stubble()
    {
        var text = _stubbleEscapeRenderer.Render("{{name}}", _stubbleEscapeModel);
        TemplateChecks.Output(LibraryCatalog.Stubble, text, TemplateData.EscapedUnsafe);
        return text;
    }
}
