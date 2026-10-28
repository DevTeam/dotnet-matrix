using Mediator;
using Microsoft.Extensions.DependencyInjection;
using Matrix.MessageDispatch.Benchmarks.MediatorLibrary;
// ReSharper disable CheckNamespace
namespace Matrix.MessageDispatch.Benchmarks;

public partial class AsyncHandler
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Mediator)]
    public async Task<AsyncResult> Mediator()
    {
        MediatorConfiguration.Counter = 0;
        var answer = await MediatorConfiguration.Mediator.Send(MediatorConfiguration.AsyncQuery);
        var result = new AsyncResult(answer, MediatorConfiguration.Counter);
        DispatchChecks.RequireAsync(LibraryCatalog.Mediator, result);
        return result;
    }
}
