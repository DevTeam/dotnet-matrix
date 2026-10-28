// ReSharper disable CheckNamespace
namespace Matrix.TemplateEngines.Benchmarks;

public partial class Render
{
    private global::Stubble.Core.StubbleVisitorRenderer _stubbleRenderer = null!;

    private const string StubbleSource = "Hello, {{name}}!";

    private object _stubbleModel = null!;

    [GlobalSetup]
    public void SetupStubble()
    {
        _stubbleRenderer = new global::Stubble.Core.Builders.StubbleBuilder().Build();
        _stubbleModel = new { name = "World" };
    }

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Stubble)]
    public string Stubble()
    {
        var text = _stubbleRenderer.Render(StubbleSource, _stubbleModel);
        TemplateChecks.Output(LibraryCatalog.Stubble, text, TemplateData.GreetingExpected);
        return text;
    }
}
