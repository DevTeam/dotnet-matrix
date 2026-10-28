using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Matrix.MessageDispatch.Benchmarks.MediatRLibrary;
// ReSharper disable CheckNamespace
namespace Matrix.MessageDispatch.Benchmarks;

public partial class Register
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.MediatR)]
    public async Task<RegistrationResult> MediatR()
    {
        await using var provider = MediatRConfiguration.BuildScope();
        var mediator = provider.GetRequiredService<IMediator>();
        var answer = await mediator.Send(MediatRConfiguration.Query);
        var result = new RegistrationResult(mediator is not null, answer);
        DispatchChecks.RequireRegistration(LibraryCatalog.MediatR, result);
        return result;
    }
}
