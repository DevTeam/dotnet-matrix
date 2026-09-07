using ValidationModules;
// ReSharper disable CheckNamespace
namespace Matrix.Validation.Benchmarks;

public partial class AsyncValidation
{
    private readonly ValidationRunner<AsyncInput> _validationModulesAsyncValidation =
        new([], [new ValidationModulesUserNameAvailability()]);

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ValidationModules)]
    public async Task<ValidationModulesResult> ValidationModules()
    {
        var result = await _validationModulesAsyncValidation.ValidateAsync(_input);
        ValidationChecks.Exact(
            LibraryCatalog.ValidationModules,
            result.IsValid,
            result.Errors.Select(error => error.Field),
            nameof(AsyncInput.UserName));
        return result;
    }
}
