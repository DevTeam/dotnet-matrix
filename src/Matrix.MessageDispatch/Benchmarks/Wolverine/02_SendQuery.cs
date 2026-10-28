using Wolverine;
using Matrix.MessageDispatch.Benchmarks.WolverineLibrary;
// ReSharper disable CheckNamespace
namespace Matrix.MessageDispatch.Benchmarks;

public partial class SendQuery
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Wolverine)]
    public async Task<QueryResult> Wolverine()
    {
        WolverineConfiguration.Counter = 0;
        WolverineConfiguration.Observed = 0;
        WolverineConfiguration.Handled = 0;
    var answer = await WolverineConfiguration.Bus.InvokeAsync<QueryAnswer>(WolverineConfiguration.Query, CancellationToken.None);
        var result = new QueryResult(answer.Value, WolverineConfiguration.Counter);
        DispatchChecks.RequireQuery(LibraryCatalog.Wolverine, result);
        return result;
    }
}
