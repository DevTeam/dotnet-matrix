using FluentValidation;

namespace Matrix.Validation.Benchmarks;

public partial class StopOnFirstFailure
{
    private readonly InlineValidator<BasicInput> _fluentStopOnFirstFailure =
        FluentValidatorFactory.Basic(CascadeMode.Stop);

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.FluentValidation)]
    public FluentValidationResult FluentValidation()
    {
        var result = _fluentStopOnFirstFailure.Validate(_input);
        ValidationChecks.Exact(
            LibraryCatalog.FluentValidation,
            result.IsValid,
            result.Errors.Select(error => error.PropertyName),
            nameof(BasicInput.Name));
        return result;
    }
}
