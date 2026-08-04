// ReSharper disable CheckNamespace
namespace Matrix.Validation.Benchmarks;

public partial class CustomRule
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.DataAnnotations)]
    public List<ValidationResult> DataAnnotations()
    {
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(
            _input,
            new ValidationContext(_input),
            results,
            true);
        ValidationChecks.Exact(
            LibraryCatalog.DataAnnotations,
            isValid,
            results.SelectMany(result => result.MemberNames),
            nameof(CustomInput.Code));
        return results;
    }
}
