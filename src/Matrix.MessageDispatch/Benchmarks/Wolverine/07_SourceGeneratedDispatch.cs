using Wolverine;
// ReSharper disable CheckNamespace
namespace Matrix.MessageDispatch.Benchmarks;

[FeatureUnavailable(
    LibraryCatalog.Wolverine,
    FeatureStatus.Unsupported,
    "WolverineFx 6.56.0 generates its handler code into a runtime-compiled assembly at start-up rather than from a compile-time source generator, and the generated code is not available to the call site, so there is no compile-time generated dispatcher to measure.")]
public partial class SourceGeneratedDispatch;
