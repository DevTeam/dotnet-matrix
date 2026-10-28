using Mediator.Net;
using Matrix.MessageDispatch.Benchmarks.MediatorNetLibrary;
// ReSharper disable CheckNamespace
namespace Matrix.MessageDispatch.Benchmarks;

public partial class SendCommand
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.MediatorNet)]
    public async Task<CommandResult> MediatorNet()
    {
        MediatorNetConfiguration.Counter = 0;
        MediatorNetConfiguration.Observed = 0;
        MediatorNetConfiguration.Handled = 0;
    await MediatorNetConfiguration.Mediator.SendAsync(MediatorNetConfiguration.Command, CancellationToken.None);
        var result = new CommandResult(MediatorNetConfiguration.Counter);
        DispatchChecks.RequireCommand(LibraryCatalog.MediatorNet, result);
        return result;
    }
}
