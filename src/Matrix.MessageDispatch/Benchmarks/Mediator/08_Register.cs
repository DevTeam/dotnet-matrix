using Mediator;
using Microsoft.Extensions.DependencyInjection;
using Matrix.MessageDispatch.Benchmarks.MediatorLibrary;
// ReSharper disable CheckNamespace
namespace Matrix.MessageDispatch.Benchmarks;

public partial class Register
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Mediator)]
    public async Task<RegistrationResult> Mediator()
    {
        await using var provider = MediatorConfiguration.BuildScope();
        var mediator = provider.GetRequiredService<IMediator>();
        var answer = await mediator.Send(MediatorConfiguration.Query);
        var result = new RegistrationResult(mediator is not null, answer);
        DispatchChecks.RequireRegistration(LibraryCatalog.Mediator, result);
        return result;
    }
}
