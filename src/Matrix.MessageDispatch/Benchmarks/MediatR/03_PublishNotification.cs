using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Matrix.MessageDispatch.Benchmarks.MediatRLibrary;
// ReSharper disable CheckNamespace
namespace Matrix.MessageDispatch.Benchmarks;

public partial class PublishNotification
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.MediatR)]
    public async Task<NotificationResult> MediatR()
    {
        MediatRConfiguration.Counter = 0;
        await MediatRConfiguration.Mediator.Publish(MediatRConfiguration.Notification);
        var result = new NotificationResult(MediatRConfiguration.Counter);
        DispatchChecks.RequireNotification(LibraryCatalog.MediatR, result);
        return result;
    }
}
