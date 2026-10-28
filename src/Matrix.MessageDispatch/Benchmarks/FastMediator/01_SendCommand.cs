using FastMediator.Core;
using Matrix.MessageDispatch.Benchmarks.FastMediatorLibrary;
// ReSharper disable CheckNamespace
namespace Matrix.MessageDispatch.Benchmarks;

public partial class SendCommand
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.FastMediator)]
    public async Task<CommandResult> FastMediator()
    {
        FastMediatorConfiguration.Counter = 0;
        FastMediatorConfiguration.Dispatcher.Send<int>(FastMediatorConfiguration.Command);
        var result = new CommandResult(FastMediatorConfiguration.Counter);
        DispatchChecks.RequireCommand(LibraryCatalog.FastMediator, result);
        return result;
    }
}
