using Immediate.Handlers.Shared;
using Matrix.MessageDispatch.Benchmarks.ImmediateLibrary;
// ReSharper disable CheckNamespace
namespace Matrix.MessageDispatch.Benchmarks;

public partial class AsyncHandler
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ImmediateHandlers)]
    public async Task<AsyncResult> ImmediateHandlers()
    {
        ImmediateConfiguration.Counter = 0;
        ImmediateConfiguration.Observed = 0;
        ImmediateConfiguration.Handled = 0;
    var answer = await ImmediateConfiguration.AsyncHandler.HandleAsync(ImmediateConfiguration.AsyncQuery, CancellationToken.None);
        var result = new AsyncResult(answer, ImmediateConfiguration.Counter);
        DispatchChecks.RequireAsync(LibraryCatalog.ImmediateHandlers, result);
        return result;
    }
}
