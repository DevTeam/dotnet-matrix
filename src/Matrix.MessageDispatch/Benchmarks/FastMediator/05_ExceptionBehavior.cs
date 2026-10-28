using FastMediator.Core;
using Matrix.MessageDispatch.Benchmarks.FastMediatorLibrary;
// ReSharper disable CheckNamespace
namespace Matrix.MessageDispatch.Benchmarks;

public partial class ExceptionBehavior
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.FastMediator)]
    public async Task<ExceptionResult> FastMediator()
    {
        FastMediatorConfiguration.Counter = 0;
        FastMediatorConfiguration.Handled = 0;
        var answer = FastMediatorConfiguration.Dispatcher.Send<int>(FastMediatorConfiguration.ThrowingQuery);
        var result = new ExceptionResult(answer, FastMediatorConfiguration.Handled == 1);
        DispatchChecks.RequireException(LibraryCatalog.FastMediator, result);
        return result;
    }
}
