using FastMediator.Core;
// ReSharper disable CheckNamespace
namespace Matrix.MessageDispatch.Benchmarks;

[FeatureUnavailable(
    LibraryCatalog.FastMediator,
    FeatureStatus.Unsupported,
    "FastMediator 2.0.1 builds its handler delegates from reflection and expression trees at start-up and ships no compile-time source generator, so there is no generated dispatch code to measure.")]
public partial class SourceGeneratedDispatch;
