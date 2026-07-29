using FluentValidation;

namespace Matrix.Validation.Benchmarks;

public partial class ValidObject
{
    private readonly InlineValidator<BasicInput> _fluentValidObject =
        FluentValidatorFactory.Basic();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.FluentValidation)]
    public FluentValidationResult FluentValidation()
    {
        var result = _fluentValidObject.Validate(_input);
        ValidationChecks.Exact(
            LibraryCatalog.FluentValidation,
            result.IsValid,
            result.Errors.Select(error => error.PropertyName));
        return result;
    }
}
