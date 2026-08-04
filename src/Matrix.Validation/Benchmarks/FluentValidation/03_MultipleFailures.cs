using FluentValidation;
// ReSharper disable CheckNamespace
namespace Matrix.Validation.Benchmarks;

public partial class MultipleFailures
{
    private readonly InlineValidator<BasicInput> _fluentMultipleFailures =
        FluentValidatorFactory.Basic();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.FluentValidation)]
    public FluentValidationResult FluentValidation()
    {
        var result = _fluentMultipleFailures.Validate(_input);
        ValidationChecks.Exact(
            LibraryCatalog.FluentValidation,
            result.IsValid,
            result.Errors.Select(error => error.PropertyName),
            nameof(BasicInput.Name),
            nameof(BasicInput.Email),
            nameof(BasicInput.Age));
        return result;
    }
}
