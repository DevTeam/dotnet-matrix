// ReSharper disable CheckNamespace
namespace Matrix.Web;

internal static class MatrixView
{
    public static bool IsSelected(
        CategoryReport report,
        IReadOnlySet<string> selectedLibraries,
        string libraryId) =>
        selectedLibraries.Contains(LibraryKey(report.Category.Id, libraryId));

    public static IEnumerable<MatrixLibrary> Libraries(CategoryReport report) =>
        (report.Features?.Libraries ?? [])
        .Concat((report.Benchmarks?.Libraries ?? [])
            .Select(library => new MatrixLibrary(
                library.Id,
                library.Name,
                library.Package,
                library.Version,
                library.Baseline)))
        .DistinctBy(library => library.Id, StringComparer.OrdinalIgnoreCase)
        .OrderBy(library => library.Name, StringComparer.OrdinalIgnoreCase);

    public static MatrixLibraryMetadata? Metadata(CategoryReport report, string libraryId) =>
        report.LibraryCatalog?.Libraries.FirstOrDefault(metadata =>
            metadata.Id.Equals(libraryId, StringComparison.OrdinalIgnoreCase));

    /// <summary>
    /// Only declared libraries compete, and only while they keep the flag.
    /// Baseline and rating are independent metadata decisions.
    /// </summary>
    public static bool IsRated(CategoryReport report, string libraryId) =>
        Metadata(report, libraryId) is { Rated: true };

    public static IReadOnlyList<MatrixMedals> Rating(
        CategoryReport report,
        IReadOnlySet<string> selectedLibraries) =>
        report.Benchmarks is null || report.ChartCatalog is null
            ? []
            : MatrixRatings.Create(
                report.Benchmarks,
                report.ChartCatalog,
                libraryId => IsRated(report, libraryId),
                libraryId => IsSelected(report, selectedLibraries, libraryId));

    /// <summary>
    /// Where the library stands in the category rating, counting from one, or
    /// null when it does not take part.
    /// </summary>
    public static int? Place(IReadOnlyList<MatrixMedals> rating, string libraryId)
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

    public static MatrixMedals? Standing(
        IReadOnlyList<MatrixMedals> rating,
        string libraryId) =>
        rating.FirstOrDefault(item =>
            item.LibraryId.Equals(libraryId, StringComparison.OrdinalIgnoreCase));

    public static string? FeatureDescription(CategoryReport report, string featureId) =>
        report.FeatureCatalog?.Features
            .FirstOrDefault(feature =>
                feature.Id.Equals(featureId, StringComparison.OrdinalIgnoreCase))
            ?.Description is { Length: > 0 } description
            ? description
            : null;

    public static string? Logo(CategoryReport report, string libraryId) =>
        Metadata(report, libraryId)?.Logo is { Length: > 0 } logo ? logo : null;

    /// <summary>
    /// The measurements behind a score, one line per scenario, for the tooltip of
    /// the number itself. A point total is only trustworthy if the reader can take
    /// it apart, and restating the formula does not let them: what they need is the
    /// two figures that produced each term. Used everywhere points are printed, so
    /// the same number always explains itself the same way.
    /// </summary>
    /// <param name="metric">False for time, true for memory, null for both.</param>
    public static string ScoreHint(
        CategoryReport report,
        IReadOnlyList<BenchmarkReportEntry> features,
        IReadOnlySet<string> selectedLibraries,
        string libraryId,
        bool? metric)
    {
        var details = MatrixScores.Explain(
            features,
            libraryId,
            candidate => IsRated(report, candidate)
                         && IsSelected(report, selectedLibraries, candidate));
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

    private static void Append(
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
        lines.Add($"{caption} — {MatrixScores.FormatExact(cells.Sum(entry => entry.Cell.Points))}"
                  + $" of {cells.Length * MatrixScores.MaximumPoints}");
        foreach (var (name, cell) in cells)
        {
            lines.Add($"  {name}: {Explain(cell, memory)}");
        }
    }

    /// <summary>
    /// One line of arithmetic. The step is named rather than folded into the
    /// numbers, because a scenario whose best result is zero is exactly where a
    /// reader stops believing the score.
    /// </summary>
    private static string Explain(MatrixScoreCell cell, bool memory)
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
        return MatrixMetrics.Format(cell.Value!.Value, memory)
               + $" against {MatrixMetrics.Format(cell.Best!.Value, memory)} best"
               + (cell.Best.Value.Equals(cell.Value.Value) ? " (equal)" : $", {step} step")
               + $" → {MatrixScores.FormatExact(cell.Points)}";
    }

    private static string LibraryKey(string categoryId, string libraryId) =>
        $"{categoryId}\u001f{libraryId}";
}
