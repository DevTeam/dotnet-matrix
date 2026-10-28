using Cortex.Mediator;
using Matrix.MessageDispatch.Benchmarks.CortexLibrary;
// ReSharper disable CheckNamespace
namespace Matrix.MessageDispatch.Benchmarks;

public partial class PublishNotification
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.CortexMediator)]
    public async Task<NotificationResult> CortexMediator()
    {
        CortexConfiguration.Counter = 0;
        CortexConfiguration.Observed = 0;
        CortexConfiguration.Handled = 0;
    await CortexConfiguration.Mediator.PublishAsync(CortexConfiguration.Notification, CancellationToken.None);
        var result = new NotificationResult(CortexConfiguration.Counter);
        DispatchChecks.RequireNotification(LibraryCatalog.CortexMediator, result);
        return result;
    }
}
