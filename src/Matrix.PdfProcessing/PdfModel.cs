namespace Matrix.PdfProcessing;

/// <summary>
/// The shared description of the documents the writing scenarios produce. Each
/// writer receives the same model instead of another library's bytes, so a
/// writing result means "this library produced this document" rather than "this
/// library re-saved someone else's file".
/// </summary>
internal static class PdfModel
{
    public const string Heading = "Matrix document";

    public const int TableRowCount = 30;

    public const int TableColumnCount = 3;

    public const int MultiPageCount = 4;

    private static readonly string[] TextDocumentLines = CreateTextDocumentLines();

    private static readonly string[] TableDocumentCells = CreateTableDocumentCells();

    private static readonly string[] MultiPageLines = CreateMultiPageLines();

    public static IReadOnlyList<string> TextDocument => TextDocumentLines;

    public static IReadOnlyList<string> TableDocument => TableDocumentCells;

    public static IReadOnlyList<string> MultiPage => MultiPageLines;

    public static string TableHeader(int column) => column switch
    {
        0 => "Id",
        1 => "Name",
        _ => "Value"
    };

    public static string TableCell(int row, int column) => column switch
    {
        0 => row.ToString(),
        1 => $"name-{row}",
        _ => $"{row * 3.5}"
    };

    public static string MultiPageHeading(int pageNumber) => $"Page {pageNumber + 1} of the document";

    public static string MultiPageLine(int pageNumber, int line) =>
        $"Page {pageNumber + 1} row {line:D2}: the quick brown fox jumps over the lazy dog.";

    private static string[] CreateTextDocumentLines()
    {
        var lines = new string[21];
        lines[0] = Heading;
        for (var line = 0; line < 20; line++)
        {
            lines[line + 1] = $"Row {line:D2}: the quick brown fox jumps over the lazy dog.";
        }

        return lines;
    }

    private static string[] CreateTableDocumentCells()
    {
        var cells = new string[TableRowCount * TableColumnCount];
        for (var row = 0; row < TableRowCount; row++)
        {
            for (var column = 0; column < TableColumnCount; column++)
            {
                cells[(row * TableColumnCount) + column] = TableCell(row, column);
            }
        }

        return cells;
    }

    private static string[] CreateMultiPageLines()
    {
        var lines = new string[MultiPageCount * (1 + 12)];
        for (var page = 0; page < MultiPageCount; page++)
        {
            var offset = page * (1 + 12);
            lines[offset] = MultiPageHeading(page);
            for (var line = 0; line < 12; line++)
            {
                lines[offset + line + 1] = MultiPageLine(page, line);
            }
        }

        return lines;
    }
}
