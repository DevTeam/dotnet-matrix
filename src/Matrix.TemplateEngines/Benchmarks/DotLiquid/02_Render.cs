// ReSharper disable CheckNamespace
namespace Matrix.TemplateEngines.Benchmarks;

public partial class Render
{
    private global::DotLiquid.Template _dotLiquidTemplate = null!;

    private global::DotLiquid.Hash _dotLiquidModel = null!;

    [GlobalSetup(Target = nameof(DotLiquid))]
    public void SetupDotLiquid()
    {
        _dotLiquidTemplate = global::DotLiquid.Template.Parse("Hello, {{ name }}!");
        _dotLiquidModel = global::DotLiquid.Hash.FromAnonymousObject(new { name = "World" });
    }

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.DotLiquid)]
    public string DotLiquid()
    {
        var text = _dotLiquidTemplate.Render(_dotLiquidModel);
        TemplateChecks.Output(LibraryCatalog.DotLiquid, text, TemplateData.GreetingExpected);
        return text;
    }
}
