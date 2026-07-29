namespace Build.Targets;

internal sealed record CiReportsOptions(
    string? Category,
    string? Libraries,
    bool Smoke,
    bool SkipBenchmarks,
    string? OutputDirectory);
