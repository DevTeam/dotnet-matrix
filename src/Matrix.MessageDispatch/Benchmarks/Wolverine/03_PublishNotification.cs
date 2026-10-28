using Wolverine;
using Matrix.MessageDispatch.Benchmarks.WolverineLibrary;
// ReSharper disable CheckNamespace
namespace Matrix.MessageDispatch.Benchmarks;

public partial class PublishNotification
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Wolverine)]
    public async Task<NotificationResult> Wolverine()
    {
        WolverineConfiguration.Counter = 0;
        WolverineConfiguration.Observed = 0;
        WolverineConfiguration.Handled = 0;
    await WolverineConfiguration.Bus.InvokeAsync(WolverineConfiguration.Notification, CancellationToken.None);
        var result = new NotificationResult(WolverineConfiguration.Counter);
        DispatchChecks.RequireNotification(LibraryCatalog.Wolverine, result);
        return result;
    }
}
