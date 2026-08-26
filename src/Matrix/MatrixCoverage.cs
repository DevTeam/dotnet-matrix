namespace Matrix;

/// <summary>
/// How many rated libraries support one scenario, computed from the validation
/// report rather than written by hand. Both the README generator and the Web
/// application call this, so the "N of M" a reader sees can never disagree
/// between the two, and can never go stale the way a number typed into a
/// contract's prose can.
/// </summary>
public static class MatrixCoverage
{
    public static (int Supported, int Rated) Feature(
        FeatureReport? features,
        MatrixLibraryMetadataCatalog? libraries,
        string featureId)
    {
        var ratedLibraryIds = (libraries?.Libraries ?? [])
            .Where(library => library.Rated)
            .Select(library => library.Id)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
        var results = features?.Features
            .FirstOrDefault(feature =>
                feature.Id.Equals(featureId, StringComparison.OrdinalIgnoreCase))
            ?.Results ?? [];
        var supported = results.Count(result =>
            result.Status == "Supported" && ratedLibraryIds.Contains(result.LibraryId));
        return (supported, ratedLibraryIds.Count);
    }
}
