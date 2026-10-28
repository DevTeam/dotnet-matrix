using Wolverine;
using Matrix.MessageDispatch.Benchmarks.WolverineLibrary;
// ReSharper disable CheckNamespace
namespace Matrix.MessageDispatch.Benchmarks;

public partial class PipelineBehavior
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Wolverine)]
    public async Task<BehaviorResult> Wolverine()
    {
        WolverineConfiguration.Counter = 0;
        WolverineConfiguration.Observed = 0;
        WolverineConfiguration.Handled = 0;
    var answer = await WolverineConfiguration.Bus.InvokeAsync<QueryAnswer>(WolverineConfiguration.BehaviorQuery, CancellationToken.None);
        var result = new BehaviorResult(answer.Value, WolverineConfiguration.Observed);
        DispatchChecks.RequireBehavior(LibraryCatalog.Wolverine, result);
        return result;
    }
}
