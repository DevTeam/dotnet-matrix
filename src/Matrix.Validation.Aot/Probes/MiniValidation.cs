using System.ComponentModel.DataAnnotations;
using MiniValidation;

namespace Matrix.Validation.Aot;

internal static class AotProbe
{
    public const string Library = "MiniValidation";

    public const int ExpectedEvents = 1;

    /// <summary>
    /// Validates one valid object and one invalid object through <c>MiniValidator.TryValidate</c>,
    /// exactly like the benchmarks' <c>ValidObject</c>/<c>SingleFailure</c> scenarios minus the
    /// shared fixture, and checks both outcomes.
    /// </summary>
    public static int Run()
    {
        var valid = MiniValidator.TryValidate(new ProbeInput { Name = "probe" }, out var validErrors);
        var invalid = MiniValidator.TryValidate(new ProbeInput { Name = string.Empty }, out var invalidErrors);

        return valid && validErrors.Count == 0 && !invalid && invalidErrors.Count == 1 ? 1 : 0;
    }

    private sealed class ProbeInput
    {
        [Required(AllowEmptyStrings = false)]
        public string Name { get; init; } = string.Empty;
    }
}
