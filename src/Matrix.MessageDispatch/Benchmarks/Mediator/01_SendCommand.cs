using Mediator;
using Microsoft.Extensions.DependencyInjection;
using Matrix.MessageDispatch.Benchmarks.MediatorLibrary;
// ReSharper disable CheckNamespace
namespace Matrix.MessageDispatch.Benchmarks;

public partial class SendCommand
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Mediator)]
    public async Task<CommandResult> Mediator()
    {
        MediatorConfiguration.Counter = 0;
        await MediatorConfiguration.Mediator.Send(MediatorConfiguration.Command);
        var result = new CommandResult(MediatorConfiguration.Counter);
        DispatchChecks.RequireCommand(LibraryCatalog.Mediator, result);
        return result;
    }
}
