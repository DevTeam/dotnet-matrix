using Mediator;
using Microsoft.Extensions.DependencyInjection;
using Matrix.MessageDispatch.Benchmarks.MediatorLibrary;
// ReSharper disable CheckNamespace
namespace Matrix.MessageDispatch.Benchmarks;

public partial class SendQuery
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Mediator)]
    public async Task<QueryResult> Mediator()
    {
        MediatorConfiguration.Counter = 0;
        var answer = await MediatorConfiguration.Mediator.Send(MediatorConfiguration.Query);
        var result = new QueryResult(answer, MediatorConfiguration.Counter);
        DispatchChecks.RequireQuery(LibraryCatalog.Mediator, result);
        return result;
    }
}
