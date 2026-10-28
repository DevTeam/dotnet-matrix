using Wolverine;
using Matrix.MessageDispatch.Benchmarks.WolverineLibrary;
// ReSharper disable CheckNamespace
namespace Matrix.MessageDispatch.Benchmarks;

public partial class SendCommand
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Wolverine)]
    public async Task<CommandResult> Wolverine()
    {
        WolverineConfiguration.Counter = 0;
        WolverineConfiguration.Observed = 0;
        WolverineConfiguration.Handled = 0;
    await WolverineConfiguration.Bus.InvokeAsync(WolverineConfiguration.Command, CancellationToken.None);
        var result = new CommandResult(WolverineConfiguration.Counter);
        DispatchChecks.RequireCommand(LibraryCatalog.Wolverine, result);
        return result;
    }
}
