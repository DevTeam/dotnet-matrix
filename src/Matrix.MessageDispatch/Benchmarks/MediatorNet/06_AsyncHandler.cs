using Mediator.Net;
using Matrix.MessageDispatch.Benchmarks.MediatorNetLibrary;
// ReSharper disable CheckNamespace
namespace Matrix.MessageDispatch.Benchmarks;

public partial class AsyncHandler
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.MediatorNet)]
    public async Task<AsyncResult> MediatorNet()
    {
        MediatorNetConfiguration.Counter = 0;
        MediatorNetConfiguration.Observed = 0;
        MediatorNetConfiguration.Handled = 0;
    var answer = await MediatorNetConfiguration.Mediator.RequestAsync<AsyncQueryMessage, QueryAnswer>(MediatorNetConfiguration.AsyncQuery, CancellationToken.None);
        var result = new AsyncResult(answer.Value, MediatorNetConfiguration.Counter);
        DispatchChecks.RequireAsync(LibraryCatalog.MediatorNet, result);
        return result;
    }
}
