// ReSharper disable CheckNamespace
namespace Matrix.TemplateEngines.Benchmarks;

public partial class Render
{
    private global::RazorEngineCore.IRazorEngineCompiledTemplate<RazorTemplate> _template = null!;

    [GlobalSetup]
    public void SetupRazorEngineCore()
    {
        var engine = new global::RazorEngineCore.RazorEngine();
        _template = engine.Compile<RazorTemplate>("Hello, @Model.Name!");
    }

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.RazorEngineCore)]
    public string RazorEngineCore()
    {
        var text = _template.Run(instance => instance.Model = TemplateData.RazorGreeting);
        TemplateChecks.Output(LibraryCatalog.RazorEngineCore, text, TemplateData.GreetingExpected);
        return text;
    }
}
