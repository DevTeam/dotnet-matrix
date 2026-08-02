// ReSharper disable NotAccessedPositionalProperty.Global
// ReSharper disable UseCollectionExpression
namespace Matrix;

/// <summary>
/// The category rating. In every scenario the fastest library scores
/// <see cref="MaximumPoints"/>, one twice as slow scores half of that, and one
/// that does not support the scenario scores nothing; a library's rating is the
/// sum. Places in the individual overview groups are still recorded, because a
/// chart row states a local fact, but they no longer decide the order.
/// The rule and the reasoning behind it are owned by workflows/rating.md.
/// </summary>
public static class MatrixRatings
{
    /// <summary>Medalled places of a single overview group.</summary>
    public const int Places = 3;

    /// <summary>What the best result in one scenario is worth.</summary>
    public const int MaximumPoints = 100;

    /// <summary>
    /// A source generator can complete a preparation scenario in a time that
    /// rounds to zero, and the ratio of two zeroes is undefined. Comparing at
    /// nanosecond resolution removes the singularity without introducing a
    /// constant anyone could argue about.
    /// </summary>
    private const double Resolution = 1;

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
        var score = Score(report.Features, libraries, Competes);
        return libraries
            .Select(library => new MatrixMedals(
                library.Id,
                library.Name,
                awards.GetValueOrDefault(library.Id) ?? [],
                (int)Math.Round(score[library.Id].Points),
                score[library.Id].Covered,
                report.Features.Count))
            .OrderByDescending(medals => medals.Points)
            .ThenBy(medals => medals.Name, StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }

    private static Dictionary<string, (double Points, int Covered)> Score(
        IEnumerable<BenchmarkReportEntry> features,
        IReadOnlyList<BenchmarkLibrary> libraries,
        Func<string, bool> competes)
    {
        var score = libraries.ToDictionary(
            library => library.Id,
            _ => (Points: 0d, Covered: 0),
            StringComparer.OrdinalIgnoreCase);
        foreach (var feature in features)
        {
            var results = feature.Results
                .Where(result =>
                    result.Successful
                    && result.MeanNanoseconds is not null
                    && competes(result.LibraryId))
                .ToArray();
            if (results.Length == 0)
            {
                continue;
            }

            var best = results.Min(result => result.MeanNanoseconds!.Value) + Resolution;
            foreach (var result in results)
            {
                if (!score.TryGetValue(result.LibraryId, out var current))
                {
                    continue;
                }

                score[result.LibraryId] = (
                    current.Points
                    + MaximumPoints * best / (result.MeanNanoseconds!.Value + Resolution),
                    current.Covered + 1);
            }
        }

        return score;
    }

    /// <summary>
    /// First three places of every overview group. These drive the stars on the
    /// chart rows; the rating itself is scored, not placed.
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

            for (var place = 0; place < Places && place < overview.Ranked.Count; place++)
            {
                var row = overview.Ranked[place];
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
