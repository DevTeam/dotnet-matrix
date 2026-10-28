using Cortex.Mediator;
using Matrix.MessageDispatch.Benchmarks.CortexLibrary;
// ReSharper disable CheckNamespace
namespace Matrix.MessageDispatch.Benchmarks;

public partial class PipelineBehavior
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.CortexMediator)]
    public async Task<BehaviorResult> CortexMediator()
    {
        CortexConfiguration.Counter = 0;
        CortexConfiguration.Observed = 0;
        CortexConfiguration.Handled = 0;
    var answer = await CortexConfiguration.Mediator.SendQueryAsync<BehaviorQueryMessage, int>(CortexConfiguration.BehaviorQuery, CancellationToken.None);
        var result = new BehaviorResult(answer, CortexConfiguration.Observed);
        DispatchChecks.RequireBehavior(LibraryCatalog.CortexMediator, result);
        return result;
    }
}
