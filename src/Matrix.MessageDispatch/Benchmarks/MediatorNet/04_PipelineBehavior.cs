using Mediator.Net;
using Matrix.MessageDispatch.Benchmarks.MediatorNetLibrary;
// ReSharper disable CheckNamespace
namespace Matrix.MessageDispatch.Benchmarks;

public partial class PipelineBehavior
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.MediatorNet)]
    public async Task<BehaviorResult> MediatorNet()
    {
        MediatorNetConfiguration.Counter = 0;
        MediatorNetConfiguration.Observed = 0;
        MediatorNetConfiguration.Handled = 0;
    var answer = await MediatorNetConfiguration.Mediator.RequestAsync<BehaviorQueryMessage, QueryAnswer>(MediatorNetConfiguration.BehaviorQuery, CancellationToken.None);
        var result = new BehaviorResult(answer.Value, MediatorNetConfiguration.Observed);
        DispatchChecks.RequireBehavior(LibraryCatalog.MediatorNet, result);
        return result;
    }
}
