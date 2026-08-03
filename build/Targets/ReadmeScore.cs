namespace Build.Targets;

/// <summary>
/// One scenario of one library's rating, already formatted. The report carries
/// the measurements as well as the points, because a total nobody can take apart
/// is a claim rather than a result.
/// </summary>
public sealed record ReadmeScore(
    string Scenario,
    string Time,
    string TimeBest,
    string TimePoints,
    string Memory,
    string MemoryBest,
    string MemoryPoints);
