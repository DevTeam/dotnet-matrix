// ReSharper disable CheckNamespace
namespace Matrix.TemplateEngines.Benchmarks;

public partial class Parse
{
    private const string RazorEngineCoreParseSource = "Hello, @Model.Name!";

    private readonly global::RazorEngineCore.RazorEngine _razorEngineCoreEngine = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.RazorEngineCore)]
    public global::RazorEngineCore.IRazorEngineCompiledTemplate<RazorTemplate> RazorEngineCore()
    {
        var template = _razorEngineCoreEngine.Compile<RazorTemplate>(RazorEngineCoreParseSource);
        TemplateChecks.NotNull(LibraryCatalog.RazorEngineCore, template);
        return template;
    }
}
