using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Validation;
using System.ComponentModel.DataAnnotations;

namespace Matrix.Validation.Aot;

internal static class AotProbe
{
    public const string Library = "Microsoft.Extensions.Validation";

    public const int ExpectedEvents = 1;

    /// <summary>
    /// Validates one valid object and one invalid object through the source-generated
    /// <c>IValidatableTypeInfo</c>, exactly like the benchmarks' <c>ValidObject</c>/<c>SingleFailure</c>
    /// scenarios minus the shared fixture, and checks both outcomes. The generator only emits
    /// metadata for a type explicitly marked <c>[ValidatableType]</c>, so <see cref="ProbeInput"/>
    /// carries it, exactly like <c>Matrix.Validation.Models.BasicInput</c> does.
    /// </summary>
    public static int Run()
    {
        using var services = new ServiceCollection().AddValidation().BuildServiceProvider();
        var options = services.GetRequiredService<IOptions<ValidationOptions>>().Value;
        if (!options.TryGetValidatableTypeInfo(typeof(ProbeInput), out var validatableInfo))
        {
            return 0;
        }

        var valid = Validate(services, validatableInfo, options, new ProbeInput { Name = "probe" });
        var invalid = Validate(services, validatableInfo, options, new ProbeInput { Name = string.Empty });

        return valid.Count == 0 && invalid.Count == 1 ? 1 : 0;
    }

    private static IReadOnlyDictionary<string, IReadOnlyList<ValidationError>> Validate(
        IServiceProvider serviceProvider,
        IValidatableTypeInfo validatableInfo,
        ValidationOptions options,
        ProbeInput input)
    {
        var context = new ValidateContext
        {
            ServiceProvider = serviceProvider,
            ValidationOptions = options
        };
        validatableInfo.ValidateAsync(input, context, CancellationToken.None)
            .GetAwaiter()
            .GetResult();
        return context.ValidationErrors
            ?? new Dictionary<string, IReadOnlyList<ValidationError>>();
    }
}

[ValidatableType]
public sealed partial class ProbeInput
{
    [Required(AllowEmptyStrings = false)]
    public string Name { get; init; } = string.Empty;
}
