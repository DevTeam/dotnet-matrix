using Matrix.CliParsing.Models;

namespace Matrix.CliParsing;

/// <summary>
/// Semantic assertions shared by every CLI adapter. Each check is compiled out of
/// benchmark builds, so the measured hot path stays a plain library call.
/// </summary>
internal static class CliChecks
{
    [Conditional("MATRIX_VALIDATION")]
    public static void RequireReport(string library, ReportArguments actual) =>
        MatrixValidation.Require(
            library,
            actual == ReportArguments.Expected,
            $"Expected {ReportArguments.Expected}, found {actual}.");

    [Conditional("MATRIX_VALIDATION")]
    public static void RequireCompact(string library, CompactArguments actual) =>
        MatrixValidation.Require(
            library,
            actual == CompactArguments.Expected,
            $"Expected {CompactArguments.Expected}, found {actual}.");

    [Conditional("MATRIX_VALIDATION")]
    public static void RequireTags(string library, TagArguments actual) =>
        MatrixValidation.Require(
            library,
            actual.Tags.SequenceEqual(TagArguments.Expected),
            $"Expected tags [{string.Join(", ", TagArguments.Expected)}], found [{string.Join(", ", actual.Tags)}].");

    [Conditional("MATRIX_VALIDATION")]
    public static void RequireSwitchOn(string library, SwitchArguments actual) =>
        MatrixValidation.Require(
            library,
            actual == SwitchArguments.Expected,
            $"Expected {SwitchArguments.Expected}, found {actual}.");

    [Conditional("MATRIX_VALIDATION")]
    public static void RequireSwitchOff(string library, SwitchArguments actual) =>
        MatrixValidation.Require(
            library,
            actual == SwitchArguments.ExpectedOff,
            $"Expected {SwitchArguments.ExpectedOff}, found {actual}.");

    [Conditional("MATRIX_VALIDATION")]
    public static void RequireHelp(string library, GeneratedHelp actual) =>
        MatrixValidation.Require(
            library,
            !string.IsNullOrWhiteSpace(actual.Text)
            && (actual.Text.Contains("--input", StringComparison.OrdinalIgnoreCase)
                || actual.Text.Contains("--format", StringComparison.OrdinalIgnoreCase)
                || actual.Text.Contains("--level", StringComparison.OrdinalIgnoreCase)),
            "Help text does not name any declared option.");

    [Conditional("MATRIX_VALIDATION")]
    public static void RequireFailure(string library, ParseOutcome actual)
    {
        MatrixValidation.Require(
            library,
            !actual.Succeeded,
            "Invalid input was accepted instead of reported.");
        MatrixValidation.Require(
            library,
            !string.IsNullOrWhiteSpace(actual.Error),
            "The failure was reported without any diagnostic message.");
    }
}
