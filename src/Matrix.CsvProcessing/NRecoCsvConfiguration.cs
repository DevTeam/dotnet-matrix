namespace Matrix.CsvProcessing;

internal static class NRecoCsvConfiguration
{
    public const string Separator = ",";

    // CsvWriter.NextRecord writes TextWriter.NewLine, so the destination carries the line ending.
    public const string NewLine = "\n";
}
