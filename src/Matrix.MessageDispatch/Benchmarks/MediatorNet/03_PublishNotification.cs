using Mediator.Net;
using Matrix.MessageDispatch.Benchmarks.MediatorNetLibrary;
// ReSharper disable CheckNamespace
namespace Matrix.MessageDispatch.Benchmarks;

public partial class PublishNotification
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.MediatorNet)]
    public async Task<NotificationResult> MediatorNet()
    {
        MediatorNetConfiguration.Counter = 0;
        MediatorNetConfiguration.Observed = 0;
        MediatorNetConfiguration.Handled = 0;
    await MediatorNetConfiguration.Mediator.PublishAsync(MediatorNetConfiguration.Notification, CancellationToken.None);
        var result = new NotificationResult(MediatorNetConfiguration.Counter);
        DispatchChecks.RequireNotification(LibraryCatalog.MediatorNet, result);
        return result;
    }
}
