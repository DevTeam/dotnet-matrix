// ReSharper disable CheckNamespace
namespace Matrix.TemplateEngines.Benchmarks;

public partial class Parse
{
    private const string ScribanParseSource = "Hello, {{ name }}!";

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Scriban)]
    public global::Scriban.Template Scriban()
    {
        var template = global::Scriban.Template.Parse(ScribanParseSource);
        TemplateChecks.NotNull(LibraryCatalog.Scriban, template);
        return template;
    }
}
