// ReSharper disable CheckNamespace
using Fluid;

namespace Matrix.TemplateEngines.Benchmarks;

public partial class Parse
{
    private const string FluidParseSource = "Hello, {{ name }}!";

    private readonly FluidParser _fluidParser = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Fluid)]
    public IFluidTemplate Fluid()
    {
        var template = _fluidParser.Parse(FluidParseSource);
        TemplateChecks.NotNull(LibraryCatalog.Fluid, template);
        return template;
    }
}
