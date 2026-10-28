using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Matrix.MessageDispatch.Benchmarks.MediatRLibrary;
// ReSharper disable CheckNamespace
namespace Matrix.MessageDispatch.Benchmarks;

public partial class SendQuery
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.MediatR)]
    public async Task<QueryResult> MediatR()
    {
        MediatRConfiguration.Counter = 0;
        var answer = await MediatRConfiguration.Mediator.Send(MediatRConfiguration.Query);
        var result = new QueryResult(answer, MediatRConfiguration.Counter);
        DispatchChecks.RequireQuery(LibraryCatalog.MediatR, result);
        return result;
    }
}
