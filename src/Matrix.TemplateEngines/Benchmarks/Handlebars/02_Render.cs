// ReSharper disable CheckNamespace
namespace Matrix.TemplateEngines.Benchmarks;

public partial class Render
{
    private global::HandlebarsDotNet.HandlebarsTemplate<object, object> _handlebarsTemplate = null!;

    private object _handlebarsModel = null!;

    [GlobalSetup]
    public void SetupHandlebars()
    {
        var handlebars = global::HandlebarsDotNet.Handlebars.Create();
        _handlebarsTemplate = handlebars.Compile("Hello, {{name}}!");
        _handlebarsModel = new { name = "World" };
    }

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Handlebars)]
    public string Handlebars()
    {
        var text = (string)_handlebarsTemplate(_handlebarsModel);
        TemplateChecks.Output(LibraryCatalog.Handlebars, text, TemplateData.GreetingExpected);
        return text;
    }
}
