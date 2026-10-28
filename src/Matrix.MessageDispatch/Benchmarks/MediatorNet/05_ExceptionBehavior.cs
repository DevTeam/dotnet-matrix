using Mediator.Net;
using Matrix.MessageDispatch.Benchmarks.MediatorNetLibrary;
// ReSharper disable CheckNamespace
namespace Matrix.MessageDispatch.Benchmarks;

public partial class ExceptionBehavior
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.MediatorNet)]
    public async Task<ExceptionResult> MediatorNet()
    {
        MediatorNetConfiguration.Counter = 0;
        MediatorNetConfiguration.Observed = 0;
        MediatorNetConfiguration.Handled = 0;
    var answer = await MediatorNetConfiguration.Mediator.RequestAsync<ThrowingQueryMessage, QueryAnswer>(MediatorNetConfiguration.ThrowingQuery, CancellationToken.None);
        var result = new ExceptionResult(answer.Value, MediatorNetConfiguration.Handled == 1);
        DispatchChecks.RequireException(LibraryCatalog.MediatorNet, result);
        return result;
    }
}
