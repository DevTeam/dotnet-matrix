using Cortex.Mediator;
using Matrix.MessageDispatch.Benchmarks.CortexLibrary;
// ReSharper disable CheckNamespace
namespace Matrix.MessageDispatch.Benchmarks;

public partial class ExceptionBehavior
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.CortexMediator)]
    public async Task<ExceptionResult> CortexMediator()
    {
        CortexConfiguration.Counter = 0;
        CortexConfiguration.Observed = 0;
        CortexConfiguration.Handled = 0;
    var answer = await CortexConfiguration.Mediator.SendQueryAsync<ThrowingQueryMessage, int>(CortexConfiguration.ThrowingQuery, CancellationToken.None);
        var result = new ExceptionResult(answer, CortexConfiguration.Handled == 1);
        DispatchChecks.RequireException(LibraryCatalog.CortexMediator, result);
        return result;
    }
}
