using Cortex.Mediator;
using Matrix.MessageDispatch.Benchmarks.CortexLibrary;
// ReSharper disable CheckNamespace
namespace Matrix.MessageDispatch.Benchmarks;

public partial class SendCommand
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.CortexMediator)]
    public async Task<CommandResult> CortexMediator()
    {
        CortexConfiguration.Counter = 0;
        CortexConfiguration.Observed = 0;
        CortexConfiguration.Handled = 0;
    await CortexConfiguration.Mediator.SendCommandAsync(CortexConfiguration.Command, CancellationToken.None);
        var result = new CommandResult(CortexConfiguration.Counter);
        DispatchChecks.RequireCommand(LibraryCatalog.CortexMediator, result);
        return result;
    }
}
