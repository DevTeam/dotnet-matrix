using Microsoft.Extensions.Validation;
// ReSharper disable CheckNamespace
namespace Matrix.Validation.Benchmarks;

public partial class Collection
{
    private readonly IValidatableInfo _microsoftCollection =
        MicrosoftExtensionsValidationConfiguration.For<CollectionInput>();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.MicrosoftExtensionsValidation)]
    public IDictionary<string, string[]>? MicrosoftExtensionsValidation()
    {
        var context = new ValidateContext
        {
            ValidationContext = new ValidationContext(_input),
            ValidationOptions = MicrosoftExtensionsValidationConfiguration.Options
        };
        _microsoftCollection.ValidateAsync(_input, context, CancellationToken.None)
            .GetAwaiter()
            .GetResult();
        var errors = context.ValidationErrors;
        ValidationChecks.Exact(
            LibraryCatalog.MicrosoftExtensionsValidation,
            errors is null or { Count: 0 },
            errors?.Keys ?? Enumerable.Empty<string>(),
            $"{nameof(CollectionInput.Items)}[1].{nameof(LineItemInput.Quantity)}");
        return errors;
    }
}
