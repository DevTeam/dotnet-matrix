using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Matrix.MessageDispatch.Benchmarks.MediatRLibrary;
// ReSharper disable CheckNamespace
namespace Matrix.MessageDispatch.Benchmarks;

public partial class SendCommand
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.MediatR)]
    public async Task<CommandResult> MediatR()
    {
        MediatRConfiguration.Counter = 0;
        await MediatRConfiguration.Mediator.Send(MediatRConfiguration.Command);
        var result = new CommandResult(MediatRConfiguration.Counter);
        DispatchChecks.RequireCommand(LibraryCatalog.MediatR, result);
        return result;
    }
}
