using Wolverine;
using Matrix.MessageDispatch.Benchmarks.WolverineLibrary;
// ReSharper disable CheckNamespace
namespace Matrix.MessageDispatch.Benchmarks;

public partial class AsyncHandler
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Wolverine)]
    public async Task<AsyncResult> Wolverine()
    {
        WolverineConfiguration.Counter = 0;
        WolverineConfiguration.Observed = 0;
        WolverineConfiguration.Handled = 0;
    var answer = await WolverineConfiguration.Bus.InvokeAsync<QueryAnswer>(WolverineConfiguration.AsyncQuery, CancellationToken.None);
        var result = new AsyncResult(answer.Value, WolverineConfiguration.Counter);
        DispatchChecks.RequireAsync(LibraryCatalog.Wolverine, result);
        return result;
    }
}
