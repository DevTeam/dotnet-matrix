using FluentValidation;

namespace Matrix.Validation.Benchmarks;

public partial class ConditionalRule
{
    private readonly InlineValidator<ConditionalInput> _fluentConditionalRule =
        FluentValidatorFactory.Conditional();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.FluentValidation)]
    public FluentValidationResult FluentValidation()
    {
        var result = _fluentConditionalRule.Validate(_input);
        ValidationChecks.Exact(
            LibraryCatalog.FluentValidation,
            result.IsValid,
            result.Errors.Select(error => error.PropertyName),
            nameof(ConditionalInput.TaxId));
        return result;
    }
}
