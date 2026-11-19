// ReSharper disable CheckNamespace
namespace Matrix.TemplateEngines.Benchmarks;

public partial class Escape
{
    private RazorBladeEscape _razorBladeEscapeTemplate = null!;

    [GlobalSetup(Target = nameof(RazorBlade))]
    public void SetupRazorBlade()
    {
        _razorBladeEscapeTemplate = new RazorBladeEscape(TemplateData.RazorUnsafe);
    }

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.RazorBlade)]
    public string RazorBlade()
    {
        var text = _razorBladeEscapeTemplate.Render();
        TemplateChecks.Output(LibraryCatalog.RazorBlade, text, TemplateData.EscapedUnsafe);
        return text;
    }
}
