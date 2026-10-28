using Cortex.Mediator;
using Microsoft.Extensions.DependencyInjection;
using Matrix.MessageDispatch.Benchmarks.CortexLibrary;
// ReSharper disable CheckNamespace
namespace Matrix.MessageDispatch.Benchmarks;

public partial class Register
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.CortexMediator)]
    public async Task<RegistrationResult> CortexMediator()
    {
        await using var provider = CortexConfiguration.BuildScope();
        var mediator = provider.GetRequiredService<IMediator>();
        var answer = await mediator.SendQueryAsync<SendQueryMessage, int>(CortexConfiguration.Query, CancellationToken.None);
        var result = new RegistrationResult(mediator is not null, answer);
        DispatchChecks.RequireRegistration(LibraryCatalog.CortexMediator, result);
        return result;
    }
}
