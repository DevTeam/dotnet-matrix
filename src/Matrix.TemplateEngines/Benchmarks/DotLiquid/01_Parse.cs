// ReSharper disable CheckNamespace
namespace Matrix.TemplateEngines.Benchmarks;

public partial class Parse
{
    private const string Source = "Hello, {{ name }}!";

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.DotLiquid)]
    public global::DotLiquid.Template DotLiquid()
    {
        var template = global::DotLiquid.Template.Parse(Source);
        TemplateChecks.NotNull(LibraryCatalog.DotLiquid, template);
        return template;
    }
}
