// ReSharper disable CheckNamespace
namespace Matrix.TemplateEngines.Benchmarks;

public partial class Render
{
    private RazorBladeGreeting _razorBladeTemplate = null!;

    [GlobalSetup(Target = nameof(RazorBlade))]
    public void SetupRazorBlade()
    {
        _razorBladeTemplate = new RazorBladeGreeting(TemplateData.RazorGreeting);
    }

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.RazorBlade)]
    public string RazorBlade()
    {
        var text = _razorBladeTemplate.Render();
        TemplateChecks.Output(LibraryCatalog.RazorBlade, text, TemplateData.GreetingExpected);
        return text;
    }
}
