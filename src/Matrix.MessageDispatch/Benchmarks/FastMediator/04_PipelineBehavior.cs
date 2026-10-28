using FastMediator.Core;
using Matrix.MessageDispatch.Benchmarks.FastMediatorLibrary;
// ReSharper disable CheckNamespace
namespace Matrix.MessageDispatch.Benchmarks;

public partial class PipelineBehavior
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.FastMediator)]
    public async Task<BehaviorResult> FastMediator()
    {
        FastMediatorConfiguration.Counter = 0;
        FastMediatorConfiguration.Observed = 0;
        var answer = FastMediatorConfiguration.Dispatcher.Send<int>(FastMediatorConfiguration.BehaviorQuery);
        var result = new BehaviorResult(answer, FastMediatorConfiguration.Observed);
        DispatchChecks.RequireBehavior(LibraryCatalog.FastMediator, result);
        return result;
    }
}
