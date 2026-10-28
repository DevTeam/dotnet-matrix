using Immediate.Handlers.Shared;
using Matrix.MessageDispatch.Benchmarks.ImmediateLibrary;
// ReSharper disable CheckNamespace
namespace Matrix.MessageDispatch.Benchmarks;

public partial class SourceGeneratedDispatch
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ImmediateHandlers)]
    public async Task<GeneratedResult> ImmediateHandlers()
    {
        ImmediateConfiguration.Counter = 0;
        var answer = await ImmediateConfiguration.QueryHandler.HandleAsync(ImmediateConfiguration.Query, CancellationToken.None);
        var result = new GeneratedResult(answer, ImmediateConfiguration.Counter);
        DispatchChecks.RequireGenerated(LibraryCatalog.ImmediateHandlers, result);
        return result;
    }
}
