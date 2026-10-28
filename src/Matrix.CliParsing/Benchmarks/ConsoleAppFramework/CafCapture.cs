using ConsoleAppFramework;

namespace Matrix.CliParsing.Benchmarks;

/// <summary>
/// Receives the values the generated handler extracts, because the generated parser
/// reports through a callback rather than through a return value. ConsoleAppFramework
/// treats a parameter without a default value as required, so every optional option
/// carries one; a repeated option overwrites the previous value instead of accumulating.
/// </summary>
internal sealed class CafCapture
{
    public CafReportValues? ReportValues { get; set; }

    public CafCompactValues? CompactValues { get; set; }

    public void Report(
        string input,
        string format = "default",
        bool verbose = false,
        string? tag = null,
        [Argument] string output = "")
        => ReportValues = new CafReportValues(input, format, verbose, tag is null ? [] : [tag], output);

    public void Compact(int level, [Argument] string input) =>
        CompactValues = new CafCompactValues(level, input);
}

internal sealed record CafReportValues(string Input, string Format, bool Verbose, string[] Tags, string Output);

internal sealed record CafCompactValues(int Level, string Input);
