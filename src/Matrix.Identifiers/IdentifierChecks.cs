namespace Matrix.Identifiers;

internal static class IdentifierChecks
{
    [Conditional("MATRIX_VALIDATION")]
    public static void Generated(string library, int length, int expectedLength) =>
        MatrixValidation.Require(
            library,
            length == expectedLength,
            $"Expected a canonical length of {expectedLength}, found {length}.");

    [Conditional("MATRIX_VALIDATION")]
    public static void Batch<T>(string library, T[] values, int expectedCount)
    {
        MatrixValidation.Require(
            library,
            values.Length == expectedCount,
            $"Expected {expectedCount} identifiers, found {values.Length}.");
        var distinct = new HashSet<T>(values);
        MatrixValidation.Require(
            library,
            distinct.Count == expectedCount,
            $"Expected {expectedCount} distinct identifiers, found {distinct.Count}.");
    }

    [Conditional("MATRIX_VALIDATION")]
    public static void Text(string library, string actual, string expected) =>
        MatrixValidation.Require(
            library,
            string.Equals(actual, expected, StringComparison.Ordinal),
            $"Expected '{expected}', found '{actual}'.");

    [Conditional("MATRIX_VALIDATION")]
    public static void TextLength(string library, string actual, int expectedLength) =>
        MatrixValidation.Require(
            library,
            actual.Length == expectedLength,
            $"Expected a text length of {expectedLength}, found {actual.Length} in '{actual}'.");

    [Conditional("MATRIX_VALIDATION")]
    public static void NotEmpty(string library, string actual) =>
        MatrixValidation.Require(
            library,
            !string.IsNullOrEmpty(actual),
            "The produced text is empty.");

    [Conditional("MATRIX_VALIDATION")]
    public static void Reflects(string library, bool same, string what) =>
        MatrixValidation.Require(library, same, $"{what} does not reflect the original value.");

    [Conditional("MATRIX_VALIDATION")]
    public static void Timestamp(string library, long actual, long expected) =>
        MatrixValidation.Require(
            library,
            actual == expected,
            $"Expected timestamp {expected}, found {actual}.");

    [Conditional("MATRIX_VALIDATION")]
    public static void TimestampAfter(string library, long actual, long minimum) =>
        MatrixValidation.Require(
            library,
            actual >= minimum,
            $"Expected a timestamp not earlier than {minimum}, found {actual}.");

    [Conditional("MATRIX_VALIDATION")]
    public static void TimestampRecent(string library, long actual, long reference, long tolerance) =>
        MatrixValidation.Require(
            library,
            Math.Abs(actual - reference) <= tolerance,
            $"Expected a timestamp within {tolerance}ms of {reference}, found {actual}.");

    [Conditional("MATRIX_VALIDATION")]
    public static void Numbers(string library, long[] actual, long[] expected)
    {
        MatrixValidation.Require(
            library,
            actual.Length == expected.Length,
            $"Expected {expected.Length} numbers, found {actual.Length}.");
        for (var index = 0; index < expected.Length; index++)
        {
            MatrixValidation.Require(
                library,
                actual[index] == expected[index],
                $"Number {index} differs. Expected {expected[index]}, found {actual[index]}.");
        }
    }

    [Conditional("MATRIX_VALIDATION")]
    public static void Numbers(string library, IReadOnlyList<long> actual, long[] expected)
    {
        MatrixValidation.Require(
            library,
            actual.Count == expected.Length,
            $"Expected {expected.Length} numbers, found {actual.Count}.");
        for (var index = 0; index < expected.Length; index++)
        {
            MatrixValidation.Require(
                library,
                actual[index] == expected[index],
                $"Number {index} differs. Expected {expected[index]}, found {actual[index]}.");
        }
    }

    [Conditional("MATRIX_VALIDATION")]
    public static void Ordered(string library, int orderedPairs, int expectedPairs) =>
        MatrixValidation.Require(
            library,
            orderedPairs == expectedPairs,
            $"Expected {expectedPairs} ordered adjacent pairs, found {orderedPairs}.");
}
