using FluentValidation;

namespace Matrix.Validation.Aot;

internal static class AotProbe
{
    public const string Library = "FluentValidation";

    public const int ExpectedEvents = 1;

    /// <summary>
    /// Validates one valid object and one invalid object through an <c>InlineValidator</c>,
    /// exactly like the benchmarks' <c>ValidObject</c>/<c>SingleFailure</c> scenarios minus the
    /// shared fixture, and checks both outcomes.
    /// </summary>
    public static int Run()
    {
        var validator = new InlineValidator<ProbeInput>();
        validator.RuleFor(input => input.Name).NotEmpty();

        var valid = validator.Validate(new ProbeInput { Name = "probe" });
        var invalid = validator.Validate(new ProbeInput { Name = string.Empty });

        return valid.IsValid && invalid.Errors.Count == 1 ? 1 : 0;
    }

    private sealed class ProbeInput
    {
        public string Name { get; init; } = string.Empty;
    }
}
