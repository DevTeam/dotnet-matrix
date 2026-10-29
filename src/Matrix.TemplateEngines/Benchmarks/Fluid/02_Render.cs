// ReSharper disable CheckNamespace
using Fluid;

namespace Matrix.TemplateEngines.Benchmarks;

public partial class Render
{
    private IFluidTemplate _fluidTemplate = null!;

    private object _fluidModel = null!;

    [GlobalSetup(Target = nameof(Fluid))]
    public void SetupFluid()
    {
        var parser = new FluidParser();
        _fluidTemplate = parser.Parse("Hello, {{ name }}!");
        _fluidModel = new { name = "World" };
    }

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Fluid)]
    public string Fluid()
    {
        var text = _fluidTemplate.Render(new TemplateContext(_fluidModel));
        TemplateChecks.Output(LibraryCatalog.Fluid, text, TemplateData.GreetingExpected);
        return text;
    }
}
