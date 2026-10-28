// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.TemplateEngines.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature("Render", 2, "Render", "Renders a template prepared in setup with a model into the final text.")]
[FeatureUnavailable(
    LibraryCatalog.RazorTemplating,
    FeatureStatus.Unsupported,
    "Razor.Templating.Core renders a view by name through the ASP.NET Core view engine, so template text cannot be prepared in setup.")]
public partial class Render;
