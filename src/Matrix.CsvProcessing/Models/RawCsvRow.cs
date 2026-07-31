namespace Matrix.CsvProcessing.Models;

public sealed record RawCsvRow(string Id, string Name, string Amount, string Active)
{
    public RawCsvRow() : this(string.Empty, string.Empty, string.Empty, string.Empty)
    {
    }
}
