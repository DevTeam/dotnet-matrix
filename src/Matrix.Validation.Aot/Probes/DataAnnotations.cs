using System.ComponentModel.DataAnnotations;

namespace Matrix.Validation.Aot;

internal static class AotProbe
{
    public const string Library = "DataAnnotations";

    public const int ExpectedEvents = 1;

    /// <summary>
    /// Validates one valid object and one invalid object through <c>Validator.TryValidateObject</c>,
    /// exactly like the benchmarks' <c>ValidObject</c>/<c>SingleFailure</c> scenarios minus the
    /// shared fixture, and checks both outcomes.
    /// </summary>
    public static int Run()
    {
        var validInput = new ProbeInput { Name = "probe" };
        var validResults = new List<ValidationResult>();
        var valid = Validator.TryValidateObject(
            validInput,
            new ValidationContext(validInput),
            validResults,
            true);

        var invalidInput = new ProbeInput { Name = string.Empty };
        var invalidResults = new List<ValidationResult>();
        var invalid = Validator.TryValidateObject(
            invalidInput,
            new ValidationContext(invalidInput),
            invalidResults,
            true);

        return valid && validResults.Count == 0 && !invalid && invalidResults.Count == 1 ? 1 : 0;
    }

    private sealed class ProbeInput
    {
        [Required(AllowEmptyStrings = false)]
        public string Name { get; init; } = string.Empty;
    }
}
