using Cortex.Mediator;
// ReSharper disable CheckNamespace
namespace Matrix.MessageDispatch.Benchmarks;

[FeatureUnavailable(
    LibraryCatalog.CortexMediator,
    FeatureStatus.Unsupported,
    "Cortex.Mediator 3.1.2 resolves handlers and behaviors from the dependency-injection container at run time and ships no compile-time source generator, so no generated dispatch code exists to measure.")]
public partial class SourceGeneratedDispatch;
