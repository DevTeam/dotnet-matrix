using Validot;
// ReSharper disable CheckNamespace
namespace Matrix.Validation.Benchmarks;

public partial class CustomRule
{
    private readonly IValidator<CustomInput> _validotCustomRule =
        ValidotSpecifications.Custom();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Validot)]
    public ValidotResult Validot()
    {
        var result = _validotCustomRule.Validate(_input);
        ValidationChecks.Exact(
            LibraryCatalog.Validot,
            !result.AnyErrors,
            result.Paths,
            nameof(CustomInput.Code));
        return result;
    }
}
