using ValidationModules;
// ReSharper disable CheckNamespace
namespace Matrix.Validation.Benchmarks;

public partial class SingleFailure
{
    private readonly BasicInputValidator _validationModulesSingleFailure = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ValidationModules)]
    public ValidationModulesResult ValidationModules()
    {
        var result = _validationModulesSingleFailure.Validate(_input);
        ValidationChecks.Exact(
            LibraryCatalog.ValidationModules,
            result.IsValid,
            result.Errors.Select(error => error.Field),
            nameof(BasicInput.Name));
        return result;
    }
}
