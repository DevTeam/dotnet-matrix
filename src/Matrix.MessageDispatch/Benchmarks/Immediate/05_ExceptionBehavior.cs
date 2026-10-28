using Immediate.Handlers.Shared;
using Matrix.MessageDispatch.Benchmarks.ImmediateLibrary;
// ReSharper disable CheckNamespace
namespace Matrix.MessageDispatch.Benchmarks;

public partial class ExceptionBehavior
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ImmediateHandlers)]
    public async Task<ExceptionResult> ImmediateHandlers()
    {
        ImmediateConfiguration.Counter = 0;
        ImmediateConfiguration.Observed = 0;
        ImmediateConfiguration.Handled = 0;
    var answer = await ImmediateConfiguration.ThrowingHandler.HandleAsync(ImmediateConfiguration.ThrowingQuery, CancellationToken.None);
        var result = new ExceptionResult(answer, ImmediateConfiguration.Handled == 1);
        DispatchChecks.RequireException(LibraryCatalog.ImmediateHandlers, result);
        return result;
    }
}
