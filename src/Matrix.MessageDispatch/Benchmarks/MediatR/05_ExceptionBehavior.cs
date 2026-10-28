using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Matrix.MessageDispatch.Benchmarks.MediatRLibrary;
// ReSharper disable CheckNamespace
namespace Matrix.MessageDispatch.Benchmarks;

public partial class ExceptionBehavior
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.MediatR)]
    public async Task<ExceptionResult> MediatR()
    {
        MediatRConfiguration.Counter = 0;
        MediatRConfiguration.Handled = 0;
        var answer = await MediatRConfiguration.Mediator.Send(MediatRConfiguration.ThrowingQuery);
        var result = new ExceptionResult(answer, MediatRConfiguration.Handled == 1);
        DispatchChecks.RequireException(LibraryCatalog.MediatR, result);
        return result;
    }
}
