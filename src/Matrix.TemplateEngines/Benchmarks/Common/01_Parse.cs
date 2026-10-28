// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.TemplateEngines.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature("Parse", 1, "Parse", "Turns template source into the engine's reusable template without rendering it.")]
[FeatureUnavailable(
    LibraryCatalog.Stubble,
    FeatureStatus.Unsupported,
    "Stubble parses into an internal token stream behind its cached parser and exposes no public pre-parsed template.")]
[FeatureUnavailable(
    LibraryCatalog.SmartFormat,
    FeatureStatus.NotApplicable,
    "SmartFormat composes a string from a format string and keeps no parsed document.")]
[FeatureUnavailable(
    LibraryCatalog.RazorLight,
    FeatureStatus.Unsupported,
    "RazorLight compiles and caches a template by name; it has no API that turns template text into a reusable template without rendering it.")]
[FeatureUnavailable(
    LibraryCatalog.RazorTemplating,
    FeatureStatus.Unsupported,
    "Razor.Templating.Core renders a view by name through the ASP.NET Core view engine and has no template string API.")]
public partial class Parse;
