using Mediator.Net;
// ReSharper disable CheckNamespace
namespace Matrix.MessageDispatch.Benchmarks;

[FeatureUnavailable(
    LibraryCatalog.MediatorNet,
    FeatureStatus.Unsupported,
    "Mediator.Net 5.0.0 builds its handler registry by reflecting over the registered assemblies at start-up and ships no compile-time source generator, so no generated dispatch code exists to measure.")]
public partial class SourceGeneratedDispatch;
