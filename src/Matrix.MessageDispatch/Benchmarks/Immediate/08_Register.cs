using Immediate.Handlers.Shared;
using Microsoft.Extensions.DependencyInjection;
using Matrix.MessageDispatch.Benchmarks.ImmediateLibrary;
// ReSharper disable CheckNamespace
namespace Matrix.MessageDispatch.Benchmarks;

public partial class Register
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ImmediateHandlers)]
    public async Task<RegistrationResult> ImmediateHandlers()
    {
        await using var provider = ImmediateConfiguration.BuildScope();
        var handler = provider.GetRequiredService<IHandler<SendQueryMessage, int>>();
        var answer = await handler.HandleAsync(ImmediateConfiguration.Query, CancellationToken.None);
        var result = new RegistrationResult(handler is not null, answer);
        DispatchChecks.RequireRegistration(LibraryCatalog.ImmediateHandlers, result);
        return result;
    }
}
