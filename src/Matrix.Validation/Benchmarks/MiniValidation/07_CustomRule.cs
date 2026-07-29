using MiniValidation;

namespace Matrix.Validation.Benchmarks;

public partial class CustomRule
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
            nameof(CustomInput.Code));
        return isValid;
    }
}
