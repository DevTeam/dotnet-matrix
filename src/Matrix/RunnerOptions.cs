namespace Matrix;

public sealed record RunnerOptions(
    string OutputFile,
    IReadOnlyList<string> Libraries,
    bool Smoke);