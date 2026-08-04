using FluentValidation;
// ReSharper disable CheckNamespace
namespace Matrix.Validation.Benchmarks;

public partial class NestedObject
{
    private readonly InlineValidator<NestedInput> _fluentNestedObject =
        FluentValidatorFactory.Nested();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.FluentValidation)]
    public FluentValidationResult FluentValidation()
    {
        var result = _fluentNestedObject.Validate(_input);
        ValidationChecks.Exact(
            LibraryCatalog.FluentValidation,
            result.IsValid,
            result.Errors.Select(error => error.PropertyName),
            $"{nameof(NestedInput.Address)}.{nameof(AddressInput.PostalCode)}");
        return result;
    }
}
