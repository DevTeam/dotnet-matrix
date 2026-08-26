// ReSharper disable CheckNamespace
namespace Matrix.Logging.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "ExceptionLogging",
    4,
    "Exception",
    "Delivers one Error event retaining the original exception metadata.")]
public partial class ExceptionLogging
{
    private readonly InvalidOperationException _exception =
        new("Database unavailable.");
}
