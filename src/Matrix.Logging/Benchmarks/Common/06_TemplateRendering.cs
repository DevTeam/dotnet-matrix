// ReSharper disable CheckNamespace
namespace Matrix.Logging.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "TemplateRendering",
    6,
    "Template Rendering",
    "Formats amount 12.5 and customer Ada through the logger template API.")]
public partial class TemplateRendering;
