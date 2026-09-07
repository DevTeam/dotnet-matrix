using ValidationModules;
// ReSharper disable CheckNamespace
namespace Matrix.Validation.Benchmarks;

public partial class ValidObject
{
    private readonly BasicInputValidator _validationModulesValidObject = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ValidationModules)]
    public ValidationModulesResult ValidationModules()
    {
        var result = _validationModulesValidObject.Validate(_input);
        ValidationChecks.Exact(
            LibraryCatalog.ValidationModules,
            result.IsValid,
            result.Errors.Select(error => error.Field));
        return result;
    }
}
