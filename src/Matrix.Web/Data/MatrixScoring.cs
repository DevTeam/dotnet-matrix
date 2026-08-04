// ReSharper disable CheckNamespace
namespace Matrix.Web;

/// <inheritdoc cref="IMatrixScoring"/>
/// <remarks>
/// Every number here comes out of <see cref="MatrixScores"/>, the one implementation
/// of the rule, which the chart renderer and the readme generator also call. A
/// breakdown shown beside a total therefore cannot disagree with it.
/// </remarks>
internal sealed class MatrixScoring(IMatrixView view, IMatrixMeasures measures) : IMatrixScoring
{
    public int Places => MatrixRatings.Places;

    public int Metrics => MatrixRatings.Metrics;

    public int MaximumPoints => MatrixRatings.MaximumPoints;

    public int Maximum(int scenarios) => MatrixScores.Maximum(scenarios);

    public string Format(double points) => MatrixScores.Format(points);

    public string FormatExact(double points) => MatrixScores.FormatExact(points);

    public IReadOnlyList<MatrixMedals> Rating(
        CategoryReport report,
        IReadOnlySet<string> selectedLibraries) =>
        report.Benchmarks is null || report.ChartCatalog is null
            ? []
            : MatrixRatings.Create(
                report.Benchmarks,
                report.ChartCatalog,
                libraryId => view.IsRated(report, libraryId),
                libraryId => view.IsSelected(report, selectedLibraries, libraryId));

    public int? Place(IReadOnlyList<MatrixMedals> rating, string libraryId)
    {
        for (var index = 0; index < rating.Count; index++)
        {
            if (rating[index].LibraryId.Equals(libraryId, StringComparison.OrdinalIgnoreCase))
            {
                return index + 1;
            }
        }

        return null;
    }

    public MatrixMedals? Standing(IReadOnlyList<MatrixMedals> rating, string libraryId) =>
        rating.FirstOrDefault(item =>
            item.LibraryId.Equals(libraryId, StringComparison.OrdinalIgnoreCase));

    public IReadOnlyList<MatrixScoreDetail> Explain(
        CategoryReport report,
        IReadOnlyList<BenchmarkReportEntry> features,
        IReadOnlySet<string> selectedLibraries,
        string libraryId) =>
        MatrixScores.Explain(
            features,
            libraryId,
            candidate => view.IsRated(report, candidate)
                         && view.IsSelected(report, selectedLibraries, candidate));

    public string Hint(
        CategoryReport report,
        IReadOnlyList<BenchmarkReportEntry> features,
        IReadOnlySet<string> selectedLibraries,
        string libraryId,
        bool? metric)
    {
        var details = Explain(report, features, selectedLibraries, libraryId);
        var lines = new List<string>();
        if (metric != true)
        {
            Append(lines, details, false, "Execution time");
        }

        if (metric != false)
        {
            Append(lines, details, true, "Allocated memory");
        }

        return string.Join('\n', lines);
    }

    private void Append(
        List<string> lines,
        IReadOnlyList<MatrixScoreDetail> details,
        bool memory,
        string caption)
    {
        if (lines.Count > 0)
        {
            lines.Add(string.Empty);
        }

        var cells = details
            .Select(detail => (detail.Name, Cell: memory ? detail.Memory : detail.Time))
            .ToArray();
        lines.Add($"{caption} — {FormatExact(cells.Sum(entry => entry.Cell.Points))}"
                  + $" of {cells.Length * MaximumPoints}");
        foreach (var (name, cell) in cells)
        {
            lines.Add($"  {name}: {Describe(cell, memory)}");
        }
    }

    /// <summary>
    /// One line of arithmetic. The step is named rather than folded into the
    /// numbers, because a scenario whose best result is zero is exactly where a
    /// reader stops believing the score.
    /// </summary>
    private string Describe(MatrixScoreCell cell, bool memory)
    {
        if (!cell.Contested)
        {
            return "nobody measured this — not scored";
        }

        if (!cell.Measured)
        {
            return "not supported → 0";
        }

        var step = memory ? $"{cell.Step:0} B" : $"{cell.Step:0} ns";
        return measures.Format(cell.Value!.Value, memory)
               + $" against {measures.Format(cell.Best!.Value, memory)} best"
               + (cell.Best.Value.Equals(cell.Value.Value) ? " (equal)" : $", {step} step")
               + $" → {FormatExact(cell.Points)}";
    }
}
