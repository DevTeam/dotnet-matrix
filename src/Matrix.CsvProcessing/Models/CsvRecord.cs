namespace Matrix.CsvProcessing.Models;

public sealed record CsvRecord(int Id, string Name, decimal Amount, bool Active)
{
    public CsvRecord() : this(0, string.Empty, 0m, false)
    {
    }
}
