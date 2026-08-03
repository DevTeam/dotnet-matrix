namespace Matrix.Logging;

internal static class LoggingChecks
{
    [Conditional("MATRIX_VALIDATION")]
    public static void Disabled(string library, int count) =>
        MatrixValidation.Require(library, count == 0, "Disabled event reached the sink.");

    [Conditional("MATRIX_VALIDATION")]
    public static void Simple(string library, CapturedLogEvent? actual)
    {
        RequireEvent(library, actual, "Information", LoggingData.SimpleMessage);
        MatrixValidation.Require(library, actual!.Exception is null, "Simple event has an exception.");
    }

    [Conditional("MATRIX_VALIDATION")]
    public static void Structured(string library, CapturedLogEvent? actual)
    {
        RequireEvent(library, actual, "Information", LoggingData.StructuredMessage);
        RequireProperty(library, actual!, "OrderId", LoggingData.OrderId);
        RequireProperty(library, actual!, "ElapsedMs", LoggingData.ElapsedMs);
    }

    [Conditional("MATRIX_VALIDATION")]
    public static void Exception(
        string library,
        CapturedLogEvent? actual,
        Exception expected)
    {
        RequireEvent(library, actual, "Error", LoggingData.ExceptionMessage);
        MatrixValidation.Same(library, expected, actual!.Exception, "Exception reference differs.");
    }

    [Conditional("MATRIX_VALIDATION")]
    public static void Scope(string library, CapturedLogEvent? actual)
    {
        RequireEvent(library, actual, "Information", LoggingData.ScopeMessage);
        RequireProperty(library, actual!, "RequestId", LoggingData.RequestId);
    }

    [Conditional("MATRIX_VALIDATION")]
    public static void Template(string library, CapturedLogEvent? actual) =>
        RequireEvent(
            library,
            actual,
            "Information",
            LoggingData.RenderedTemplateMessage);

    [Conditional("MATRIX_VALIDATION")]
    public static void Buffered(string library, int count, CapturedLogEvent? actual)
    {
        MatrixValidation.Require(library, count == 3, $"Expected 3 buffered events, found {count}.");
        RequireEvent(library, actual, "Information", LoggingData.BufferedMessage);
    }

    [Conditional("MATRIX_VALIDATION")]
    public static void FileAppended(string library, int lineCount, string? lastLine)
    {
        MatrixValidation.Require(
            library,
            lineCount == 3,
            $"Expected 3 appended lines, found {lineCount}.");
        MatrixValidation.Require(
            library,
            lastLine is not null && lastLine.EndsWith(LoggingData.FileMessage, StringComparison.Ordinal),
            $"Last appended line does not end with '{LoggingData.FileMessage}': '{lastLine}'.");
    }

    [Conditional("MATRIX_VALIDATION")]
    public static void Prepared(string library, bool informationEnabled) =>
        MatrixValidation.Require(
            library,
            informationEnabled,
            "Prepared logger does not enable Information.");

    private static void RequireEvent(
        string library,
        CapturedLogEvent? actual,
        string level,
        string message)
    {
        MatrixValidation.Require(library, actual is not null, "No event reached the sink.");
        MatrixValidation.Require(
            library,
            actual!.Level == level,
            $"Expected level '{level}', found '{actual.Level}'.");
        MatrixValidation.Require(
            library,
            actual.RenderedMessage == message,
            $"Expected message '{message}', found '{actual.RenderedMessage}'.");
    }

    private static void RequireProperty(
        string library,
        CapturedLogEvent actual,
        string name,
        object expected)
    {
        MatrixValidation.Require(
            library,
            actual.Properties.TryGetValue(name, out var value),
            $"Property '{name}' was not captured.");
        MatrixValidation.Require(
            library,
            Equals(value, expected),
            $"Property '{name}' expected '{expected}', found '{value}'.");
    }
}

