namespace Build.Targets;

/// <summary>
/// The model for one category's full report (<c>reports/&lt;Category&gt;/README.md</c>).
/// <see cref="Category"/> carries paths relative to its own report directory, not
/// the solution root — see <see cref="ReadmeTarget"/>'s rerooting of the same
/// <see cref="ReadmeCategory"/> used for the summary in the top-level README.
/// </summary>
public sealed record CategoryReportModel(
    string ApplicationUrl,
    ReadmeCategory Category);
