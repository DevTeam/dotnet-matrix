// ReSharper disable NotAccessedPositionalProperty.Global
// ReSharper disable UseCollectionExpression
namespace Matrix;

/// <summary>
/// Selection, ordering and scaling for a single scenario. The readme renderer draws
/// one table with both metrics side by side; the web application draws two ranked
/// lists. Both take their rows and their bar lengths from here.
/// </summary>
public static class MatrixScenarios
{
    public static IReadOnlyList<MatrixScenarioRow> Create(
        BenchmarkReport report,
        BenchmarkReportEntry feature,
        Func<string, bool>? includeLibrary = null)
    {
        var names = report.Libraries.ToDictionary(
            library => library.Id,
            library => library.Name,
            StringComparer.OrdinalIgnoreCase);
        return feature.Results
            .Where(result => result.Successful)
            .Where(result =>
                result.MeanNanoseconds is not null
                || result.AllocatedBytesPerOperation is not null)
            .Where(result => includeLibrary?.Invoke(result.LibraryId) ?? true)
            .Select(result => new MatrixScenarioRow(
                result.LibraryId,
                names.GetValueOrDefault(result.LibraryId, result.LibraryId),
                result.MeanNanoseconds,
                result.StandardErrorNanoseconds,
                result.AllocatedBytesPerOperation,
                result.EnvironmentId))
            .ToArray();
    }

    /// <summary>
    /// Best first. Rows without a value for the metric sort last, so a list stays
    /// complete even when only one of the two metrics was captured.
    /// </summary>
    public static IReadOnlyList<MatrixScenarioRow> Order(
        IEnumerable<MatrixScenarioRow> rows,
        bool memory) =>
        rows
            .OrderBy(row => Value(row, memory) ?? double.MaxValue)
            .ThenBy(row => row.Name, StringComparer.OrdinalIgnoreCase)
            .ToArray();

    public static double? Value(MatrixScenarioRow row, bool memory) =>
        memory ? row.Memory : row.Time;

    public static double Maximum(IEnumerable<MatrixScenarioRow> rows, bool memory) =>
        rows.Select(row => Value(row, memory) ?? 0).DefaultIfEmpty().Max();

    public static double Minimum(IEnumerable<MatrixScenarioRow> rows, bool memory) =>
        rows
            .Select(row => Value(row, memory))
            .Where(value => value is not null)
            .Select(value => value!.Value)
            .DefaultIfEmpty()
            .Min();
}
