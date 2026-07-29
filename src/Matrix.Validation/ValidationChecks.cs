namespace Matrix.Validation;

internal static class ValidationChecks
{
    [Conditional("MATRIX_VALIDATION")]
    public static void Exact(
        string library,
        bool isValid,
        IEnumerable<string> actualPaths,
        params string[] expectedPaths)
    {
        var actual = actualPaths.Order(StringComparer.Ordinal).ToArray();
        var expected = expectedPaths.Order(StringComparer.Ordinal).ToArray();
        MatrixValidation.Require(
            library,
            isValid == (expected.Length == 0),
            expected.Length == 0
                ? "Expected a valid result."
                : "Expected an invalid result.");
        MatrixValidation.Require(
            library,
            actual.SequenceEqual(expected, StringComparer.Ordinal),
            $"Expected paths [{string.Join(", ", expected)}], "
            + $"but found [{string.Join(", ", actual)}].");
    }
}
