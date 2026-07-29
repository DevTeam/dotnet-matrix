// ReSharper disable NotAccessedPositionalProperty.Global
namespace Matrix;

/// <summary>
/// One medal: the overview group it was won in and the place, 1 to 3.
/// </summary>
public sealed record MatrixMedal(
    string GroupId,
    string GroupName,
    int Place);

/// <summary>
/// What one library won across the overview groups of a category.
/// </summary>
public sealed record MatrixMedals(
    string LibraryId,
    string Name,
    IReadOnlyList<MatrixMedal> Awards)
{
    public int Gold => Count(1);

    public int Silver => Count(2);

    public int Bronze => Count(3);

    public int Total => Awards.Count;

    public int Count(int place) => Awards.Count(award => award.Place == place);
}

/// <summary>
/// Medals are derived from the overview rankings rather than computed separately:
/// first place in a group takes gold, second silver, third bronze. The number of
/// groups comes from the chart catalog, so adding one to charts.json adds a medal
/// of every colour without touching this code.
/// </summary>
public static class MatrixRatings
{
    public const int Places = 3;

    public static IReadOnlyList<MatrixMedals> Create(
        BenchmarkReport report,
        MatrixChartCatalog charts,
        Func<string, bool> isRated,
        Func<string, bool>? includeLibrary = null)
    {
        var medals = new Dictionary<string, List<MatrixMedal>>(StringComparer.OrdinalIgnoreCase);
        var names = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var group in charts.Groups)
        {
            var overview = MatrixOverviews.Create(
                report,
                group,
                libraryId => isRated(libraryId) && (includeLibrary?.Invoke(libraryId) ?? true));
            if (overview is null)
            {
                continue;
            }

            for (var place = 0; place < Places && place < overview.Ranked.Count; place++)
            {
                var row = overview.Ranked[place];
                names[row.LibraryId] = row.Name;
                if (!medals.TryGetValue(row.LibraryId, out var awards))
                {
                    awards = [];
                    medals[row.LibraryId] = awards;
                }

                awards.Add(new MatrixMedal(group.Id, group.Name, place + 1));
            }
        }

        return medals
            .Select(entry => new MatrixMedals(
                entry.Key,
                names[entry.Key],
                entry.Value
                    .OrderBy(award => award.Place)
                    .ThenBy(award => award.GroupName, StringComparer.OrdinalIgnoreCase)
                    .ToArray()))
            .OrderByDescending(item => item.Gold)
            .ThenByDescending(item => item.Silver)
            .ThenByDescending(item => item.Bronze)
            .ThenBy(item => item.Name, StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }
}
