using Mediator;
using Microsoft.Extensions.DependencyInjection;
using Matrix.MessageDispatch.Benchmarks.MediatorLibrary;
// ReSharper disable CheckNamespace
namespace Matrix.MessageDispatch.Benchmarks;

public partial class ExceptionBehavior
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Mediator)]
    public async Task<ExceptionResult> Mediator()
    {
        MediatorConfiguration.Counter = 0;
        MediatorConfiguration.Handled = 0;
        var answer = await MediatorConfiguration.Mediator.Send(MediatorConfiguration.ThrowingQuery);
        var result = new ExceptionResult(answer, MediatorConfiguration.Handled == 1);
        DispatchChecks.RequireException(LibraryCatalog.Mediator, result);
        return result;
    }
}
