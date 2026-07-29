namespace Build.Targets;

internal sealed record CiReportsOptions(
    string? Libraries,
    bool Smoke,
    bool SkipBenchmarks,
    string? OutputDirectory);
