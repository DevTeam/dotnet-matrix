// ReSharper disable CheckNamespace
namespace Matrix.TemplateEngines.Benchmarks;

public partial class Escape
{
    private const string RazorLightEscapeSource = "@Model.Name";

    private global::RazorLight.RazorLightEngine _razorLightEscapeEngine = null!;

    private object _razorLightEscapeModel = null!;

    [GlobalSetup(Target = nameof(RazorLight))]
    public void SetupRazorLight()
    {
        _razorLightEscapeEngine = new global::RazorLight.RazorLightEngineBuilder()
            .UseNoProject()
            .UseMemoryCachingProvider()
            .Build();
        _razorLightEscapeModel = new { Name = TemplateData.RawUnsafe };
    }

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.RazorLight)]
    public string RazorLight()
    {
        var text = _razorLightEscapeEngine
            .CompileRenderStringAsync("escape", RazorLightEscapeSource, _razorLightEscapeModel)
            .GetAwaiter()
            .GetResult();
        TemplateChecks.Output(LibraryCatalog.RazorLight, text, TemplateData.EscapedUnsafe);
        return text;
    }
}
