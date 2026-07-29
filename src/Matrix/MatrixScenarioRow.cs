namespace Matrix;

/// <summary>
/// One library's result for a single benchmark scenario.
/// </summary>
public sealed record MatrixScenarioRow(
    string LibraryId,
    string Name,
    double? Time,
    double? TimeStandardError,
    double? Memory,
    string? EnvironmentId);