// ReSharper disable CheckNamespace
namespace Matrix.TemplateEngines.Benchmarks;

public partial class Render
{
    private const string RazorLightSource = "Hello, @Model.Name!";

    private global::RazorLight.RazorLightEngine _razorLightEngine = null!;

    private object _razorLightModel = null!;

    [GlobalSetup]
    public void SetupRazorLight()
    {
        _razorLightEngine = new global::RazorLight.RazorLightEngineBuilder()
            .UseNoProject()
            .UseMemoryCachingProvider()
            .Build();
        _razorLightModel = new { Name = "World" };
    }

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.RazorLight)]
    public string RazorLight()
    {
        var text = _razorLightEngine
            .CompileRenderStringAsync("render", RazorLightSource, _razorLightModel)
            .GetAwaiter()
            .GetResult();
        TemplateChecks.Output(LibraryCatalog.RazorLight, text, TemplateData.GreetingExpected);
        return text;
    }
}
