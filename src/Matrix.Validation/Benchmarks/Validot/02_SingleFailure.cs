using Validot;
// ReSharper disable CheckNamespace
namespace Matrix.Validation.Benchmarks;

public partial class SingleFailure
{
    private readonly IValidator<BasicInput> _validotSingleFailure =
        ValidotSpecifications.Basic();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Validot)]
    public ValidotResult Validot()
    {
        var result = _validotSingleFailure.Validate(_input);
        ValidationChecks.Exact(
            LibraryCatalog.Validot,
            !result.AnyErrors,
            result.Paths,
            nameof(BasicInput.Name));
        return result;
    }
}
