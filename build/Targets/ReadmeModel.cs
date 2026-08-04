namespace Build.Targets;

public sealed record ReadmeModel(
    string ApplicationUrl,
    IReadOnlyList<ReadmeCategory> Categories)
{
    /// <summary>
    /// Real values from the current report, so the documented links stay clickable
    /// instead of naming a category that was renamed or removed.
    /// </summary>
    public string SampleCategory =>
        Categories.Count == 0 ? "csv-processing" : Categories[0].Id;

    public string SampleLibrary =>
        Categories
            .SelectMany(category => category.Libraries)
            .FirstOrDefault()
            ?.Id
        ?? "sep";
}
