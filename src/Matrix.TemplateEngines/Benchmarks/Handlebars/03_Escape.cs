// ReSharper disable CheckNamespace
namespace Matrix.TemplateEngines.Benchmarks;

public partial class Escape
{
    private global::HandlebarsDotNet.HandlebarsTemplate<object, object> _handlebarsEscapeTemplate = null!;

    private object _handlebarsEscapeModel = null!;

    [GlobalSetup(Target = nameof(Handlebars))]
    public void SetupHandlebars()
    {
        var handlebars = global::HandlebarsDotNet.Handlebars.Create();
        _handlebarsEscapeTemplate = handlebars.Compile("{{name}}");
        _handlebarsEscapeModel = new { name = TemplateData.RawUnsafe };
    }

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Handlebars)]
    public string Handlebars()
    {
        var text = (string)_handlebarsEscapeTemplate(_handlebarsEscapeModel);
        TemplateChecks.Output(LibraryCatalog.Handlebars, text, TemplateData.EscapedUnsafe);
        return text;
    }
}
