namespace Matrix.CsvProcessing.Models;

public sealed record TextCsvRow(int Id, string Text)
{
    public TextCsvRow() : this(0, string.Empty)
    {
    }
}
