using FastMediator.Core;
using Microsoft.Extensions.DependencyInjection;
using Matrix.MessageDispatch.Benchmarks.FastMediatorLibrary;
// ReSharper disable CheckNamespace
namespace Matrix.MessageDispatch.Benchmarks;

public partial class Register
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.FastMediator)]
    public async Task<RegistrationResult> FastMediator()
    {
        await using var provider = FastMediatorConfiguration.BuildScope();
        var dispatcher = provider.GetRequiredService<Dispatcher>();
        var answer = dispatcher.Send<int>(FastMediatorConfiguration.Query);
        var result = new RegistrationResult(dispatcher is not null, answer);
        DispatchChecks.RequireRegistration(LibraryCatalog.FastMediator, result);
        return result;
    }
}
