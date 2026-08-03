// ReSharper disable NotAccessedPositionalProperty.Global
// ReSharper disable UseCollectionExpression
namespace Matrix;

/// <summary>
/// The category rating: <see cref="MatrixScores"/> applied to every scenario of
/// the report. An overview group applies the same rule to its own scenarios, so
/// a group standing and the category standing differ only in what they cover.
/// Group places are still recorded here for the stars on the chart rows.
/// The rule and the reasoning are owned by workflows/rating.md.
/// </summary>
public static class MatrixRatings
{
    /// <summary>Medalled places of a standing.</summary>
    public const int Places = 3;

    /// <inheritdoc cref="MatrixScores.MaximumPoints"/>
    public const int MaximumPoints = MatrixScores.MaximumPoints;

    /// <inheritdoc cref="MatrixScores.Metrics"/>
    public const int Metrics = MatrixScores.Metrics;

    public static IReadOnlyList<MatrixMedals> Create(
        BenchmarkReport report,
        MatrixChartCatalog charts,
        Func<string, bool> isRated,
        Func<string, bool>? includeLibrary = null)
    {
        bool Competes(string libraryId) =>
            isRated(libraryId) && (includeLibrary?.Invoke(libraryId) ?? true);

        var libraries = report.Libraries
            .Where(library => Competes(library.Id))
            .ToArray();
        if (libraries.Length == 0)
        {
            return [];
        }

        var awards = ReadAwards(report, charts, Competes);
        var score = MatrixScores.Create(
            report.Features,
            libraries.Select(library => library.Id),
            Competes);
        return libraries
            .Select(library =>
            {
                var earned = score.GetValueOrDefault(library.Id) ?? new MatrixScore(0, 0, 0);
                return new MatrixMedals(
                    library.Id,
                    library.Name,
                    awards.GetValueOrDefault(library.Id) ?? [],
                    earned.Time,
                    earned.Memory,
                    earned.Covered,
                    report.Features.Count);
            })
            .OrderByDescending(medals => medals.Points)
            .ThenBy(medals => medals.Name, StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }

    /// <summary>
    /// First three places of every overview group, by the points scored in that
    /// group. The rows of a group are already in that order.
    /// </summary>
    private static Dictionary<string, List<MatrixMedal>> ReadAwards(
        BenchmarkReport report,
        MatrixChartCatalog charts,
        Func<string, bool> competes)
    {
        var awards = new Dictionary<string, List<MatrixMedal>>(StringComparer.OrdinalIgnoreCase);
        foreach (var group in charts.Groups)
        {
            var overview = MatrixOverviews.Create(report, group, competes);
            if (overview is null)
            {
                continue;
            }

            for (var place = 0; place < Places && place < overview.Rows.Count; place++)
            {
                var row = overview.Rows[place];
                if (!awards.TryGetValue(row.LibraryId, out var won))
                {
                    won = [];
                    awards[row.LibraryId] = won;
                }

                won.Add(new MatrixMedal(group.Id, group.Name, place + 1));
            }
        }

        foreach (var won in awards.Values)
        {
            won.Sort((left, right) =>
                left.Place != right.Place
                    ? left.Place.CompareTo(right.Place)
                    : string.Compare(
                        left.GroupName,
                        right.GroupName,
                        StringComparison.OrdinalIgnoreCase));
        }

        return awards;
    }
}
