using Wolverine;
using Microsoft.Extensions.DependencyInjection;
using Matrix.MessageDispatch.Benchmarks.WolverineLibrary;
// ReSharper disable CheckNamespace
namespace Matrix.MessageDispatch.Benchmarks;

public partial class Register
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Wolverine)]
    public async Task<RegistrationResult> Wolverine()
    {
        using var host = WolverineConfiguration.BuildScope();
        var bus = host.Services.GetRequiredService<IMessageBus>();
        var answer = await bus.InvokeAsync<QueryAnswer>(WolverineConfiguration.Query, CancellationToken.None);
        var result = new RegistrationResult(bus is not null, answer.Value);
        DispatchChecks.RequireRegistration(LibraryCatalog.Wolverine, result);
        return result;
    }
}
