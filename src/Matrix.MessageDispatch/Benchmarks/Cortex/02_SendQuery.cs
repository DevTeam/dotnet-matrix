using Cortex.Mediator;
using Matrix.MessageDispatch.Benchmarks.CortexLibrary;
// ReSharper disable CheckNamespace
namespace Matrix.MessageDispatch.Benchmarks;

public partial class SendQuery
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.CortexMediator)]
    public async Task<QueryResult> CortexMediator()
    {
        CortexConfiguration.Counter = 0;
        CortexConfiguration.Observed = 0;
        CortexConfiguration.Handled = 0;
    var answer = await CortexConfiguration.Mediator.SendQueryAsync<SendQueryMessage, int>(CortexConfiguration.Query, CancellationToken.None);
        var result = new QueryResult(answer, CortexConfiguration.Counter);
        DispatchChecks.RequireQuery(LibraryCatalog.CortexMediator, result);
        return result;
    }
}
