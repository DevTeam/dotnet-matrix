// ReSharper disable CheckNamespace
namespace Matrix.TemplateEngines.Benchmarks;

public partial class Parse
{
    private const string HandlebarsParseSource = "Hello, {{name}}!";

    private readonly global::HandlebarsDotNet.IHandlebars _handlebars = global::HandlebarsDotNet.Handlebars.Create();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Handlebars)]
    public global::HandlebarsDotNet.HandlebarsTemplate<object, object> Handlebars()
    {
        var template = _handlebars.Compile(HandlebarsParseSource);
        TemplateChecks.NotNull(LibraryCatalog.Handlebars, template);
        return template;
    }
}
