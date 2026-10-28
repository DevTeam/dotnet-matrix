using Immediate.Handlers.Shared;
using Matrix.MessageDispatch.Benchmarks.ImmediateLibrary;
// ReSharper disable CheckNamespace
namespace Matrix.MessageDispatch.Benchmarks;

public partial class SendCommand
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ImmediateHandlers)]
    public async Task<CommandResult> ImmediateHandlers()
    {
        ImmediateConfiguration.Counter = 0;
        ImmediateConfiguration.Observed = 0;
        ImmediateConfiguration.Handled = 0;
    await ImmediateConfiguration.CommandHandler.HandleAsync(ImmediateConfiguration.Command, CancellationToken.None);
        var result = new CommandResult(ImmediateConfiguration.Counter);
        DispatchChecks.RequireCommand(LibraryCatalog.ImmediateHandlers, result);
        return result;
    }
}
