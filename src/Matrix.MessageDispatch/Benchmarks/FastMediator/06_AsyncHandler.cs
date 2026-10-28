using FastMediator.Core;
using Matrix.MessageDispatch.Benchmarks.FastMediatorLibrary;
// ReSharper disable CheckNamespace
namespace Matrix.MessageDispatch.Benchmarks;

public partial class AsyncHandler
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.FastMediator)]
    public async Task<AsyncResult> FastMediator()
    {
        FastMediatorConfiguration.Counter = 0;
        var answer = await FastMediatorConfiguration.Dispatcher.SendAsync(FastMediatorConfiguration.AsyncQuery, CancellationToken.None);
        var result = new AsyncResult(answer, FastMediatorConfiguration.Counter);
        DispatchChecks.RequireAsync(LibraryCatalog.FastMediator, result);
        return result;
    }
}
