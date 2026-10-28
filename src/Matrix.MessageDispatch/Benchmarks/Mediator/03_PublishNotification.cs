using Mediator;
using Microsoft.Extensions.DependencyInjection;
using Matrix.MessageDispatch.Benchmarks.MediatorLibrary;
// ReSharper disable CheckNamespace
namespace Matrix.MessageDispatch.Benchmarks;

public partial class PublishNotification
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Mediator)]
    public async Task<NotificationResult> Mediator()
    {
        MediatorConfiguration.Counter = 0;
        await MediatorConfiguration.Mediator.Publish(MediatorConfiguration.Notification);
        var result = new NotificationResult(MediatorConfiguration.Counter);
        DispatchChecks.RequireNotification(LibraryCatalog.Mediator, result);
        return result;
    }
}
