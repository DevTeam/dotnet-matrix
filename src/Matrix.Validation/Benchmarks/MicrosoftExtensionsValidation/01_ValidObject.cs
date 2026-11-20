using Microsoft.Extensions.Validation;
// ReSharper disable CheckNamespace
namespace Matrix.Validation.Benchmarks;

public partial class ValidObject
{
    private readonly IValidatableTypeInfo _microsoftValidObject =
        MicrosoftExtensionsValidationConfiguration.For<BasicInput>();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.MicrosoftExtensionsValidation)]
    public IReadOnlyDictionary<string, IReadOnlyList<ValidationError>>? MicrosoftExtensionsValidation()
    {
        var context = new ValidateContext
        {
            ServiceProvider = MicrosoftExtensionsValidationConfiguration.ServiceProvider,
            ValidationOptions = MicrosoftExtensionsValidationConfiguration.Options
        };
        _microsoftValidObject.ValidateAsync(_input, context, CancellationToken.None)
            .GetAwaiter()
            .GetResult();
        var errors = context.ValidationErrors;
        ValidationChecks.Exact(
            LibraryCatalog.MicrosoftExtensionsValidation,
            errors is null or { Count: 0 },
            errors?.Keys ?? Enumerable.Empty<string>());
        return errors;
    }
}
