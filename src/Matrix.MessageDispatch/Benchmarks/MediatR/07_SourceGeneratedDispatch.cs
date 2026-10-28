using MediatR;
// ReSharper disable CheckNamespace
namespace Matrix.MessageDispatch.Benchmarks;

[FeatureUnavailable(
    LibraryCatalog.MediatR,
    FeatureStatus.Unsupported,
    "MediatR 14 resolves handlers and behaviors through the container at run time and ships no compile-time source generator, so no generated dispatch code exists to measure.")]
public partial class SourceGeneratedDispatch;
