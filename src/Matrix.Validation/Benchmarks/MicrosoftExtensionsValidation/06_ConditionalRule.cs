using Microsoft.Extensions.Validation;

namespace Matrix.Validation.Benchmarks;

public partial class ConditionalRule
{
    private readonly IValidatableInfo _microsoftConditionalRule =
        MicrosoftExtensionsValidationConfiguration.For<ConditionalInput>();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.MicrosoftExtensionsValidation)]
    public IDictionary<string, string[]>? MicrosoftExtensionsValidation()
    {
        var context = new ValidateContext
        {
            ValidationContext = new ValidationContext(_input),
            ValidationOptions = MicrosoftExtensionsValidationConfiguration.Options
        };
        _microsoftConditionalRule.ValidateAsync(_input, context, default)
            .GetAwaiter()
            .GetResult();
        var errors = context.ValidationErrors;
        ValidationChecks.Exact(
            LibraryCatalog.MicrosoftExtensionsValidation,
            errors is null or { Count: 0 },
            errors?.Keys ?? Enumerable.Empty<string>(),
            nameof(ConditionalInput.TaxId));
        return errors;
    }
}
