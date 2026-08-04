// ReSharper disable CheckNamespace
namespace Matrix.Validation.Benchmarks;

public partial class NestedObject
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.DataAnnotations)]
    public (List<ValidationResult> Root, List<ValidationResult> Address) DataAnnotations()
    {
        var rootResults = new List<ValidationResult>();
        var rootIsValid = Validator.TryValidateObject(
            _input,
            new ValidationContext(_input),
            rootResults,
            true);
        var addressResults = new List<ValidationResult>();
        var addressIsValid = Validator.TryValidateObject(
            _input.Address,
            new ValidationContext(_input.Address),
            addressResults,
            true);
        ValidationChecks.Exact(
            LibraryCatalog.DataAnnotations,
            rootIsValid && addressIsValid,
            rootResults
                .SelectMany(result => result.MemberNames)
                .Concat(addressResults
                    .SelectMany(result => result.MemberNames)
                    .Select(path => $"{nameof(NestedInput.Address)}.{path}")),
            $"{nameof(NestedInput.Address)}.{nameof(AddressInput.PostalCode)}");
        return (rootResults, addressResults);
    }
}
