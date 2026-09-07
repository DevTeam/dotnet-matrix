using ValidationModules;
// ReSharper disable CheckNamespace
namespace Matrix.Validation.Benchmarks;

public partial class Collection
{
    private readonly CollectionInputValidator _validationModulesCollection =
        new([new LineItemInputValidator()]);

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ValidationModules)]
    public ValidationModulesResult ValidationModules()
    {
        var result = _validationModulesCollection.Validate(_input);
        ValidationChecks.Exact(
            LibraryCatalog.ValidationModules,
            result.IsValid,
            result.Errors.Select(error => error.Field),
            $"{nameof(CollectionInput.Items)}[1].{nameof(LineItemInput.Quantity)}");
        return result;
    }
}
