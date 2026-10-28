using Matrix.MessageDispatch.Models;

namespace Matrix.MessageDispatch;

/// <summary>
/// Semantic assertions shared by every dispatcher adapter. Each check is compiled out
/// of benchmark builds, so the measured hot path stays a plain library call.
/// </summary>
internal static class DispatchChecks
{
    [Conditional("MATRIX_VALIDATION")]
    public static void RequireCommand(string library, CommandResult actual) =>
        MatrixValidation.Require(
            library,
            actual == CommandResult.Expected,
            $"Expected {CommandResult.Expected}, found {actual}.");

    [Conditional("MATRIX_VALIDATION")]
    public static void RequireQuery(string library, QueryResult actual) =>
        MatrixValidation.Require(
            library,
            actual == QueryResult.Expected,
            $"Expected {QueryResult.Expected}, found {actual}.");

    [Conditional("MATRIX_VALIDATION")]
    public static void RequireNotification(string library, NotificationResult actual) =>
        MatrixValidation.Require(
            library,
            actual == NotificationResult.Expected,
            $"Expected {NotificationResult.Expected} handler invocations, found {actual.Invocations}.");

    [Conditional("MATRIX_VALIDATION")]
    public static void RequireBehavior(string library, BehaviorResult actual) =>
        MatrixValidation.Require(
            library,
            actual == BehaviorResult.Expected,
            $"Expected {BehaviorResult.Expected}, found {actual}.");

    [Conditional("MATRIX_VALIDATION")]
    public static void RequireException(string library, ExceptionResult actual) =>
        MatrixValidation.Require(
            library,
            actual == ExceptionResult.Expected,
            $"Expected {ExceptionResult.Expected}, found {actual}.");

    [Conditional("MATRIX_VALIDATION")]
    public static void RequireAsync(string library, AsyncResult actual) =>
        MatrixValidation.Require(
            library,
            actual == AsyncResult.Expected,
            $"Expected {AsyncResult.Expected}, found {actual}.");

    [Conditional("MATRIX_VALIDATION")]
    public static void RequireGenerated(string library, GeneratedResult actual) =>
        MatrixValidation.Require(
            library,
            actual == GeneratedResult.Expected,
            $"Expected {GeneratedResult.Expected}, found {actual}.");

    [Conditional("MATRIX_VALIDATION")]
    public static void RequireRegistration(string library, RegistrationResult actual) =>
        MatrixValidation.Require(
            library,
            actual == RegistrationResult.Expected,
            $"Expected {RegistrationResult.Expected}, found {actual}.");
}
