using MiniValidation;
// ReSharper disable CheckNamespace
namespace Matrix.Validation.Benchmarks;

public partial class Collection
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.MiniValidation)]
    public bool MiniValidation()
    {
        var isValid = MiniValidator.TryValidate(_input, out var errors);
        ValidationChecks.Exact(
            LibraryCatalog.MiniValidation,
            isValid,
            errors.Keys,
            $"{nameof(CollectionInput.Items)}[1].{nameof(LineItemInput.Quantity)}");
        return isValid;
    }
}
