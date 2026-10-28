// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.TemplateEngines.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature("Escape", 3, "Escape", "Renders text containing markup, which the engine must HTML-escape.")]
[FeatureUnavailable(
    LibraryCatalog.Scriban,
    FeatureStatus.NotApplicable,
    "Scriban does not escape by default; escaping requires the explicit html.escape filter, which is a different scenario.")]
[FeatureUnavailable(
    LibraryCatalog.Fluid,
    FeatureStatus.NotApplicable,
    "Fluid does not escape by default; escaping requires the explicit escape filter.")]
[FeatureUnavailable(
    LibraryCatalog.DotLiquid,
    FeatureStatus.NotApplicable,
    "DotLiquid does not escape by default; escaping requires the explicit escape filter.")]
[FeatureUnavailable(
    LibraryCatalog.SmartFormat,
    FeatureStatus.NotApplicable,
    "SmartFormat composes strings and applies no HTML escaping.")]
[FeatureUnavailable(
    LibraryCatalog.Cottle,
    FeatureStatus.NotApplicable,
    "Cottle emits values verbatim and applies no escaping.")]
[FeatureUnavailable(
    LibraryCatalog.RazorEngineCore,
    FeatureStatus.NotApplicable,
    "RazorEngineCore writes model values verbatim, without the HTML encoder of a Razor page.")]
[FeatureUnavailable(
    LibraryCatalog.RazorTemplating,
    FeatureStatus.Unsupported,
    "Razor.Templating.Core renders a view by name through the ASP.NET Core view engine and has no template string API.")]
public partial class Escape;
