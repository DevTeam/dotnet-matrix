using Wolverine;
// ReSharper disable CheckNamespace
namespace Matrix.MessageDispatch.Benchmarks;

[FeatureUnavailable(
    LibraryCatalog.Wolverine,
    FeatureStatus.Unsupported,
    "WolverineFx 6.56.0 runs a [WolverineOnException] continuation when the handler of that message fails, but the inline InvokeAsync caller still completes with a null response instead of the continuation's value, so a recovered answer cannot be returned to the caller of the request.")]
public partial class ExceptionBehavior;
