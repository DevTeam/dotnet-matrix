using Microsoft.Extensions.Validation;
// ReSharper disable CheckNamespace
namespace Matrix.Validation.Benchmarks;

public partial class SingleFailure
{
    private readonly IValidatableTypeInfo _microsoftSingleFailure =
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
        _microsoftSingleFailure.ValidateAsync(_input, context, CancellationToken.None)
            .GetAwaiter()
            .GetResult();
        var errors = context.ValidationErrors;
        ValidationChecks.Exact(
            LibraryCatalog.MicrosoftExtensionsValidation,
            errors is null or { Count: 0 },
            errors?.Keys ?? Enumerable.Empty<string>(),
            nameof(BasicInput.Name));
        return errors;
    }
}
