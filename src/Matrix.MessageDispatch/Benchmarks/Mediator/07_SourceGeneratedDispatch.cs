using Mediator;
using Microsoft.Extensions.DependencyInjection;
using Matrix.MessageDispatch.Benchmarks.MediatorLibrary;
// ReSharper disable CheckNamespace
namespace Matrix.MessageDispatch.Benchmarks;

public partial class SourceGeneratedDispatch
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Mediator)]
    public async Task<GeneratedResult> Mediator()
    {
        MediatorConfiguration.Counter = 0;
        var answer = await MediatorConfiguration.Mediator.Send(MediatorConfiguration.Query);
        var result = new GeneratedResult(answer, MediatorConfiguration.Counter);
        DispatchChecks.RequireGenerated(LibraryCatalog.Mediator, result);
        return result;
    }
}
