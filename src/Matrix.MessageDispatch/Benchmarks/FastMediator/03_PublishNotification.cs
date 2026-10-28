using FastMediator.Core;
// ReSharper disable CheckNamespace
namespace Matrix.MessageDispatch.Benchmarks;

[FeatureUnavailable(
    LibraryCatalog.FastMediator,
    FeatureStatus.Unsupported,
    "FastMediator 2.0.1 passes the whole notification handler list to every notification handler, so three registered handlers are invoked three times each instead of once; its fan-out cannot produce the one-invocation-per-handler result this scenario requires.")]
public partial class PublishNotification;
