using ValidationModules;
// ReSharper disable CheckNamespace
namespace Matrix.Validation.Benchmarks;

public partial class MultipleFailures
{
    private readonly BasicInputValidator _validationModulesMultipleFailures = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ValidationModules)]
    public ValidationModulesResult ValidationModules()
    {
        var result = _validationModulesMultipleFailures.Validate(_input);
        ValidationChecks.Exact(
            LibraryCatalog.ValidationModules,
            result.IsValid,
            result.Errors.Select(error => error.Field),
            nameof(BasicInput.Name),
            nameof(BasicInput.Email),
            nameof(BasicInput.Age));
        return result;
    }
}
