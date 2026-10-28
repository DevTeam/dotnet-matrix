using Cortex.Mediator;
using Matrix.MessageDispatch.Benchmarks.CortexLibrary;
// ReSharper disable CheckNamespace
namespace Matrix.MessageDispatch.Benchmarks;

public partial class AsyncHandler
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.CortexMediator)]
    public async Task<AsyncResult> CortexMediator()
    {
        CortexConfiguration.Counter = 0;
        CortexConfiguration.Observed = 0;
        CortexConfiguration.Handled = 0;
    var answer = await CortexConfiguration.Mediator.SendQueryAsync<AsyncQueryMessage, int>(CortexConfiguration.AsyncQuery, CancellationToken.None);
        var result = new AsyncResult(answer, CortexConfiguration.Counter);
        DispatchChecks.RequireAsync(LibraryCatalog.CortexMediator, result);
        return result;
    }
}
