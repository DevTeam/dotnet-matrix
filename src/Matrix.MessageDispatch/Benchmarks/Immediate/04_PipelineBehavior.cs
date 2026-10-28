using Immediate.Handlers.Shared;
using Matrix.MessageDispatch.Benchmarks.ImmediateLibrary;
// ReSharper disable CheckNamespace
namespace Matrix.MessageDispatch.Benchmarks;

public partial class PipelineBehavior
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ImmediateHandlers)]
    public async Task<BehaviorResult> ImmediateHandlers()
    {
        ImmediateConfiguration.Counter = 0;
        ImmediateConfiguration.Observed = 0;
        ImmediateConfiguration.Handled = 0;
    var answer = await ImmediateConfiguration.BehaviorHandler.HandleAsync(ImmediateConfiguration.BehaviorQuery, CancellationToken.None);
        var result = new BehaviorResult(answer, ImmediateConfiguration.Observed);
        DispatchChecks.RequireBehavior(LibraryCatalog.ImmediateHandlers, result);
        return result;
    }
}
