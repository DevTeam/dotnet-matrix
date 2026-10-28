namespace Matrix.TemplateEngines;

internal static class TemplateChecks
{
    [Conditional("MATRIX_VALIDATION")]
    public static void Output(string library, string actual, string expected) =>
        MatrixValidation.Require(
            library,
            string.Equals(actual, expected, StringComparison.Ordinal),
            $"Expected '{expected}', found '{actual}'.");

    [Conditional("MATRIX_VALIDATION")]
    public static void NotEmpty(string library, string actual) =>
        MatrixValidation.Require(
            library,
            !string.IsNullOrEmpty(actual),
            "The produced template is empty.");

    [Conditional("MATRIX_VALIDATION")]
    public static void NotNull(string library, object? value) =>
        MatrixValidation.Require(library, value is not null, "The parsed template is null.");
}
