using ValidationModules;
// ReSharper disable CheckNamespace
namespace Matrix.Validation.Benchmarks;

public partial class ConditionalRule
{
    private readonly ConditionalInputValidator _validationModulesConditionalRule = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ValidationModules)]
    public ValidationModulesResult ValidationModules()
    {
        var result = _validationModulesConditionalRule.Validate(_input);
        ValidationChecks.Exact(
            LibraryCatalog.ValidationModules,
            result.IsValid,
            result.Errors.Select(error => error.Field),
            nameof(ConditionalInput.TaxId));
        return result;
    }
}
