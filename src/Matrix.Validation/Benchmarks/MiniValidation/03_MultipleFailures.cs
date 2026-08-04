using MiniValidation;
// ReSharper disable CheckNamespace
namespace Matrix.Validation.Benchmarks;

public partial class MultipleFailures
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
            nameof(BasicInput.Name),
            nameof(BasicInput.Email),
            nameof(BasicInput.Age));
        return isValid;
    }
}
