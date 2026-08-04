using FluentValidation;
// ReSharper disable CheckNamespace
namespace Matrix.Validation.Benchmarks;

public partial class AsyncValidation
{
    private readonly InlineValidator<AsyncInput> _fluentAsyncValidation =
        FluentValidatorFactory.Async();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.FluentValidation)]
    public async Task<FluentValidationResult> FluentValidation()
    {
        var result = await _fluentAsyncValidation.ValidateAsync(_input);
        ValidationChecks.Exact(
            LibraryCatalog.FluentValidation,
            result.IsValid,
            result.Errors.Select(error => error.PropertyName),
            nameof(AsyncInput.UserName));
        return result;
    }
}
