using FluentValidation;
// ReSharper disable CheckNamespace
namespace Matrix.Validation.Benchmarks;

public partial class SingleFailure
{
    private readonly InlineValidator<BasicInput> _fluentSingleFailure =
        FluentValidatorFactory.Basic();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.FluentValidation)]
    public FluentValidationResult FluentValidation()
    {
        var result = _fluentSingleFailure.Validate(_input);
        ValidationChecks.Exact(
            LibraryCatalog.FluentValidation,
            result.IsValid,
            result.Errors.Select(error => error.PropertyName),
            nameof(BasicInput.Name));
        return result;
    }
}
