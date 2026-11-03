using System.Globalization;
using NReco.Csv;
// ReSharper disable CheckNamespace
namespace Matrix.CsvProcessing.Benchmarks;

public partial class HeaderMapping
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.NRecoCsv)]
    public CsvRecord[] NRecoCsv()
    {
        var reader = new CsvReader(new StringReader(_csv), NRecoCsvConfiguration.Separator);
        var id = -1;
        var name = -1;
        var amount = -1;
        var active = -1;
        if (reader.Read())
        {
            for (var column = 0; column < reader.FieldsCount; column++)
            {
                switch (reader[column])
                {
                    case "Id":
                        id = column;
                        break;
                    case "Name":
                        name = column;
                        break;
                    case "Amount":
                        amount = column;
                        break;
                    case "Active":
                        active = column;
                        break;
                }
            }
        }

        var records = new List<CsvRecord>(3);
        while (reader.Read())
        {
            records.Add(new CsvRecord(
                int.Parse(reader[id], NumberStyles.Integer, CultureInfo.InvariantCulture),
                reader[name],
                decimal.Parse(reader[amount], NumberStyles.Number, CultureInfo.InvariantCulture),
                bool.Parse(reader[active])));
        }

        var result = records.ToArray();
        CsvChecks.Records(LibraryCatalog.NRecoCsv, result);
        return result;
    }
}
