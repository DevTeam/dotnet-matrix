using Mediator.Net;
using Matrix.MessageDispatch.Benchmarks.MediatorNetLibrary;
// ReSharper disable CheckNamespace
namespace Matrix.MessageDispatch.Benchmarks;

public partial class Register
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.MediatorNet)]
    public async Task<RegistrationResult> MediatorNet()
    {
        var mediator = MediatorNetConfiguration.BuildScope();
        var answer = await mediator.RequestAsync<SendQueryMessage, QueryAnswer>(MediatorNetConfiguration.Query, CancellationToken.None);
        var result = new RegistrationResult(mediator is not null, answer.Value);
        DispatchChecks.RequireRegistration(LibraryCatalog.MediatorNet, result);
        return result;
    }
}
