using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Validation;

namespace Matrix.Validation.Benchmarks;

internal static class MicrosoftExtensionsValidationConfiguration
{
    private static readonly ServiceProvider Services = CreateServices();

    public static ValidationOptions Options { get; } =
        Services.GetRequiredService<IOptions<ValidationOptions>>().Value;

    public static IValidatableInfo For<T>()
    {
        if (Options.TryGetValidatableTypeInfo(typeof(T), out var validatableInfo))
        {
            return validatableInfo;
        }

        throw new InvalidOperationException(
            $"No generated validation metadata was found for '{typeof(T)}'.");
    }

    private static ServiceProvider CreateServices()
    {
        var services = new ServiceCollection();
        services.AddValidation();
        return services.BuildServiceProvider();
    }
}
