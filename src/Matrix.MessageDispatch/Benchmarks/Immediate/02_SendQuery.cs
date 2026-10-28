using Immediate.Handlers.Shared;
using Matrix.MessageDispatch.Benchmarks.ImmediateLibrary;
// ReSharper disable CheckNamespace
namespace Matrix.MessageDispatch.Benchmarks;

public partial class SendQuery
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ImmediateHandlers)]
    public async Task<QueryResult> ImmediateHandlers()
    {
        ImmediateConfiguration.Counter = 0;
        ImmediateConfiguration.Observed = 0;
        ImmediateConfiguration.Handled = 0;
    var answer = await ImmediateConfiguration.QueryHandler.HandleAsync(ImmediateConfiguration.Query, CancellationToken.None);
        var result = new QueryResult(answer, ImmediateConfiguration.Counter);
        DispatchChecks.RequireQuery(LibraryCatalog.ImmediateHandlers, result);
        return result;
    }
}
