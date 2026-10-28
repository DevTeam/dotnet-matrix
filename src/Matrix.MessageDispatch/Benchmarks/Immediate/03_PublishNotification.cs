using Immediate.Handlers.Shared;
// ReSharper disable CheckNamespace
namespace Matrix.MessageDispatch.Benchmarks;

[FeatureUnavailable(
    LibraryCatalog.ImmediateHandlers,
    FeatureStatus.Unsupported,
    "Immediate.Handlers 4.2.0 generates one IHandler per annotated handler method and has no notification contract, so one message cannot be delivered to several handlers.")]
public partial class PublishNotification;
