using MiniValidation;

namespace Matrix.Validation.Benchmarks;

public partial class SingleFailure
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
            nameof(BasicInput.Name));
        return isValid;
    }
}
