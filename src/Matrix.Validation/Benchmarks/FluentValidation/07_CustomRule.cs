using FluentValidation;

namespace Matrix.Validation.Benchmarks;

public partial class CustomRule
{
    private readonly InlineValidator<CustomInput> _fluentCustomRule =
        FluentValidatorFactory.Custom();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.FluentValidation)]
    public FluentValidationResult FluentValidation()
    {
        var result = _fluentCustomRule.Validate(_input);
        ValidationChecks.Exact(
            LibraryCatalog.FluentValidation,
            result.IsValid,
            result.Errors.Select(error => error.PropertyName),
            nameof(CustomInput.Code));
        return result;
    }
}
