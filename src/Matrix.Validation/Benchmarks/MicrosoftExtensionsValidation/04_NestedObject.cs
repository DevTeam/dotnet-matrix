using Microsoft.Extensions.Validation;

namespace Matrix.Validation.Benchmarks;

public partial class NestedObject
{
    private readonly IValidatableInfo _microsoftNestedObject =
        MicrosoftExtensionsValidationConfiguration.For<NestedInput>();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.MicrosoftExtensionsValidation)]
    public IDictionary<string, string[]>? MicrosoftExtensionsValidation()
    {
        var context = new ValidateContext
        {
            ValidationContext = new ValidationContext(_input),
            ValidationOptions = MicrosoftExtensionsValidationConfiguration.Options
        };
        _microsoftNestedObject.ValidateAsync(_input, context, default)
            .GetAwaiter()
            .GetResult();
        var errors = context.ValidationErrors;
        ValidationChecks.Exact(
            LibraryCatalog.MicrosoftExtensionsValidation,
            errors is null or { Count: 0 },
            errors?.Keys ?? Enumerable.Empty<string>(),
            $"{nameof(NestedInput.Address)}.{nameof(AddressInput.PostalCode)}");
        return errors;
    }
}
