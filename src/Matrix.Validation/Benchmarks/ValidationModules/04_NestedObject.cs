using ValidationModules;
// ReSharper disable CheckNamespace
namespace Matrix.Validation.Benchmarks;

public partial class NestedObject
{
    private readonly NestedInputValidator _validationModulesNestedObject =
        new([new AddressInputValidator()]);

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ValidationModules)]
    public ValidationModulesResult ValidationModules()
    {
        var result = _validationModulesNestedObject.Validate(_input);
        ValidationChecks.Exact(
            LibraryCatalog.ValidationModules,
            result.IsValid,
            result.Errors.Select(error => error.Field),
            $"{nameof(NestedInput.Address)}.{nameof(AddressInput.PostalCode)}");
        return result;
    }
}
