using Mediator;
using Microsoft.Extensions.DependencyInjection;
using Matrix.MessageDispatch.Benchmarks.MediatorLibrary;
// ReSharper disable CheckNamespace
namespace Matrix.MessageDispatch.Benchmarks;

public partial class PipelineBehavior
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Mediator)]
    public async Task<BehaviorResult> Mediator()
    {
        MediatorConfiguration.Counter = 0;
        MediatorConfiguration.Observed = 0;
        var answer = await MediatorConfiguration.Mediator.Send(MediatorConfiguration.BehaviorQuery);
        var result = new BehaviorResult(answer, MediatorConfiguration.Observed);
        DispatchChecks.RequireBehavior(LibraryCatalog.Mediator, result);
        return result;
    }
}
