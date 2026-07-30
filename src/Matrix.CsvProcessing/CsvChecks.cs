namespace Matrix.CsvProcessing;

internal static class CsvChecks
{
    [Conditional("MATRIX_VALIDATION")]
    public static void RawRows(string library, RawCsvRow[] actual)
    {
        MatrixValidation.Require(library, actual.Length == 3, "Expected three raw rows.");
        MatrixValidation.Require(
            library,
            actual[0] == new RawCsvRow("1", "Ada", "12.50", "true"),
            "First raw row differs.");
        MatrixValidation.Require(
            library,
            actual[1] == new RawCsvRow("2", "Grace", "7.25", "false"),
            "Second raw row differs.");
        MatrixValidation.Require(
            library,
            actual[2] == new RawCsvRow("3", "Linus", "100.00", "true"),
            "Third raw row differs.");
    }

    [Conditional("MATRIX_VALIDATION")]
    public static void Records(string library, CsvRecord[] actual)
    {
        MatrixValidation.Require(library, actual.Length == 3, "Expected three typed records.");
        MatrixValidation.Require(
            library,
            actual[0] == new CsvRecord(1, "Ada", 12.50m, true),
            "First typed record differs.");
        MatrixValidation.Require(
            library,
            actual[1] == new CsvRecord(2, "Grace", 7.25m, false),
            "Second typed record differs.");
        MatrixValidation.Require(
            library,
            actual[2] == new CsvRecord(3, "Linus", 100.00m, true),
            "Third typed record differs.");
    }

    [Conditional("MATRIX_VALIDATION")]
    public static void Large(string library, CsvRecord[] actual)
    {
        MatrixValidation.Require(library, actual.Length == 10_000, "Large row count differs.");
        MatrixValidation.Require(
            library,
            actual[0] == new CsvRecord(1, "Name1", 0.0m, false),
            "First large record differs.");
        MatrixValidation.Require(
            library,
            actual[^1] == new CsvRecord(10_000, "Name10000", 999.9m, true),
            "Last large record differs.");
        Aggregate(library, Aggregate(actual));
    }

    [Conditional("MATRIX_VALIDATION")]
    public static void Quoted(string library, TextCsvRow[] actual)
    {
        MatrixValidation.Require(library, actual.Length == 2, "Quoted row count differs.");
        MatrixValidation.Require(
            library,
            actual[0] == new TextCsvRow(1, "Ada \"Countess\""),
            "First quoted row differs.");
        MatrixValidation.Require(
            library,
            actual[1] == new TextCsvRow(2, "\"quoted\""),
            "Second quoted row differs.");
    }

    [Conditional("MATRIX_VALIDATION")]
    public static void EscapedDelimiters(string library, TextCsvRow[] actual)
    {
        MatrixValidation.Require(
            library,
            actual.Length == 2,
            "Escaped-delimiter row count differs.");
        MatrixValidation.Require(
            library,
            actual[0] == new TextCsvRow(1, "Ada, Lovelace"),
            "Embedded delimiter was not preserved.");
        MatrixValidation.Require(
            library,
            actual[1] == new TextCsvRow(2, "line one\nline two"),
            "Embedded newline was not preserved.");
    }

    [Conditional("MATRIX_VALIDATION")]
    public static void ProductCodes(string library, ProductCode[] actual)
    {
        MatrixValidation.Require(library, actual.Length == 2, "Product-code count differs.");
        MatrixValidation.Require(
            library,
            actual[0] == new ProductCode(42) && actual[1] == new ProductCode(73),
            "Product-code conversion differs.");
    }

    [Conditional("MATRIX_VALIDATION")]
    public static void Aggregate(string library, CsvAggregate actual) =>
        MatrixValidation.Require(
            library,
            actual == CsvData.LargeAggregate,
            $"Aggregate differs. Expected {CsvData.LargeAggregate}, found {actual}.");

    [Conditional("MATRIX_VALIDATION")]
    public static void WrittenCsv(string library, string actual) =>
        MatrixValidation.Require(
            library,
            string.Equals(actual, CsvData.WriteCsv, StringComparison.Ordinal),
            $"Written CSV differs. Expected '{Escape(CsvData.WriteCsv)}', found '{Escape(actual)}'.");

    private static CsvAggregate Aggregate(IEnumerable<CsvRecord> records)
    {
        var count = 0;
        long idSum = 0;
        decimal amountSum = 0;
        var activeCount = 0;
        foreach (var record in records)
        {
            count++;
            idSum += record.Id;
            amountSum += record.Amount;
            if (record.Active)
            {
                activeCount++;
            }
        }

        return new CsvAggregate(count, idSum, amountSum, activeCount);
    }

    private static string Escape(string value) =>
        value.Replace("\n", "\\n", StringComparison.Ordinal);
}

