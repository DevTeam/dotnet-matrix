using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Matrix.MessageDispatch.Benchmarks.MediatRLibrary;
// ReSharper disable CheckNamespace
namespace Matrix.MessageDispatch.Benchmarks;

public partial class PipelineBehavior
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.MediatR)]
    public async Task<BehaviorResult> MediatR()
    {
        MediatRConfiguration.Counter = 0;
        MediatRConfiguration.Observed = 0;
        var answer = await MediatRConfiguration.Mediator.Send(MediatRConfiguration.BehaviorQuery);
        var result = new BehaviorResult(answer, MediatRConfiguration.Observed);
        DispatchChecks.RequireBehavior(LibraryCatalog.MediatR, result);
        return result;
    }
}
