// ReSharper disable CheckNamespace
namespace Matrix.MessageDispatch.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "Register",
    8,
    "Register",
    "Builds the container, registers the handlers and resolves the dispatcher.")]
public partial class Register
{
}
