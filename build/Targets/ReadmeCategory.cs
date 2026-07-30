namespace Build.Targets;

public sealed record ReadmeCategory(
    string Anchor,
    string Name,
    IReadOnlyList<ReadmeLibrary> Libraries,
    IReadOnlyList<ReadmeChart> Overviews,
    IReadOnlyList<ReadmeFeature> Features,
    IReadOnlyList<ReadmeRating> Rating);
