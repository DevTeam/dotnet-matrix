// ReSharper disable CheckNamespace
namespace Matrix.TemplateEngines.Benchmarks;

public partial class Render
{
    private global::Scriban.Template _scribanTemplate = null!;

    [GlobalSetup(Target = nameof(Scriban))]
    public void SetupScriban() =>
        _scribanTemplate = global::Scriban.Template.Parse("Hello, {{ name }}!");

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Scriban)]
    public string Scriban()
    {
        var script = new global::Scriban.Runtime.ScriptObject();
        script.Add("name", "World");
        var context = new global::Scriban.TemplateContext();
        context.PushGlobal(script);
        var text = _scribanTemplate.Render(context);
        TemplateChecks.Output(LibraryCatalog.Scriban, text, TemplateData.GreetingExpected);
        return text;
    }
}
