using Cocona;

namespace Matrix.CliParsing.Benchmarks;

/// <summary>
/// Receives the values the Cocona command delegate extracts. Cocona binds arguments to
/// delegate parameters and reports a parse failure by writing to the error writer and
/// setting an exit code instead of throwing.
/// </summary>
internal sealed class CoconaCapture
{
    public CoconaReportValues? ReportValues { get; set; }

    public CoconaCompactValues? CompactValues { get; set; }

    public void Report(string input, string format, bool verbose, string[]? tag, [Argument] string output) =>
        ReportValues = new CoconaReportValues(input, format, verbose, tag ?? [], output);

    public void Compact(int level, [Argument] string input) =>
        CompactValues = new CoconaCompactValues(level, input);
}

internal sealed record CoconaReportValues(string Input, string Format, bool Verbose, string[] Tags, string Output);

internal sealed record CoconaCompactValues(int Level, string Input);
