using Mediator.Net;
using Matrix.MessageDispatch.Benchmarks.MediatorNetLibrary;
// ReSharper disable CheckNamespace
namespace Matrix.MessageDispatch.Benchmarks;

public partial class SendQuery
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.MediatorNet)]
    public async Task<QueryResult> MediatorNet()
    {
        MediatorNetConfiguration.Counter = 0;
        MediatorNetConfiguration.Observed = 0;
        MediatorNetConfiguration.Handled = 0;
    var answer = await MediatorNetConfiguration.Mediator.RequestAsync<SendQueryMessage, QueryAnswer>(MediatorNetConfiguration.Query, CancellationToken.None);
        var result = new QueryResult(answer.Value, MediatorNetConfiguration.Counter);
        DispatchChecks.RequireQuery(LibraryCatalog.MediatorNet, result);
        return result;
    }
}
