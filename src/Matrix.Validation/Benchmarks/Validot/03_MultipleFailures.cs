using Validot;
// ReSharper disable CheckNamespace
namespace Matrix.Validation.Benchmarks;

public partial class MultipleFailures
{
    private readonly IValidator<BasicInput> _validotMultipleFailures =
        ValidotSpecifications.Basic();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Validot)]
    public ValidotResult Validot()
    {
        var result = _validotMultipleFailures.Validate(_input);
        ValidationChecks.Exact(
            LibraryCatalog.Validot,
            !result.AnyErrors,
            result.Paths,
            nameof(BasicInput.Name),
            nameof(BasicInput.Email),
            nameof(BasicInput.Age));
        return result;
    }
}
