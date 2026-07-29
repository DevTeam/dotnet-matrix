namespace Matrix;

public static class MatrixValidation
{
    public static void Require(bool condition, string message)
    {
        if (!condition)
        {
            throw new InvalidOperationException(message);
        }
    }

    public static void Require(string library, bool condition, string message) =>
        Require(condition, $"{library}: {message}");

    public static void Same(object? left, object? right, string message) =>
        Require(ReferenceEquals(left, right), message);

    public static void Same(string library, object? left, object? right, string message) =>
        Same(left, right, $"{library}: {message}");

    public static void Different(object? left, object? right, string message) =>
        Require(!ReferenceEquals(left, right), message);

    public static void Different(string library, object? left, object? right, string message) =>
        Different(left, right, $"{library}: {message}");
}
