using FastMediator.Core;
using Matrix.MessageDispatch.Benchmarks.FastMediatorLibrary;
// ReSharper disable CheckNamespace
namespace Matrix.MessageDispatch.Benchmarks;

public partial class SendQuery
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.FastMediator)]
    public async Task<QueryResult> FastMediator()
    {
        FastMediatorConfiguration.Counter = 0;
        var answer = FastMediatorConfiguration.Dispatcher.Send<int>(FastMediatorConfiguration.Query);
        var result = new QueryResult(answer, FastMediatorConfiguration.Counter);
        DispatchChecks.RequireQuery(LibraryCatalog.FastMediator, result);
        return result;
    }
}
