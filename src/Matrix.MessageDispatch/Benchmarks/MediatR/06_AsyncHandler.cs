using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Matrix.MessageDispatch.Benchmarks.MediatRLibrary;
// ReSharper disable CheckNamespace
namespace Matrix.MessageDispatch.Benchmarks;

public partial class AsyncHandler
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.MediatR)]
    public async Task<AsyncResult> MediatR()
    {
        MediatRConfiguration.Counter = 0;
        var answer = await MediatRConfiguration.Mediator.Send(MediatRConfiguration.AsyncQuery);
        var result = new AsyncResult(answer, MediatRConfiguration.Counter);
        DispatchChecks.RequireAsync(LibraryCatalog.MediatR, result);
        return result;
    }
}
