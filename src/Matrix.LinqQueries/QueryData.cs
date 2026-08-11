namespace Matrix.LinqQueries;

internal static class QueryData
{
    public static readonly int[] Numbers;
    public static readonly List<int> NumberList;
    public static readonly int[] ScanNumbers;
    public static readonly int[][] Batches;
    public static readonly Order[] Orders;
    public static readonly Customer[] Customers;
    public static readonly string[] Regions =
    [
        "North", "South", "East", "West",
        "Nordic", "Alpine", "Baltic", "Iberia"
    ];

    static QueryData()
    {
        Numbers = new int[10_000];
        ScanNumbers = new int[10_000];
        for (var i = 0; i < Numbers.Length; i++)
        {
            Numbers[i] = i * 37 % 1000;
            ScanNumbers[i] = i * 37 % 500;
        }

        ScanNumbers[9_500] = 777;
        NumberList = new List<int>(Numbers);

        Batches = new int[500][];
        for (var batch = 0; batch < Batches.Length; batch++)
        {
            var values = new int[20];
            for (var index = 0; index < values.Length; index++)
            {
                values[index] = (batch * values.Length + index) * 37 % 1000;
            }

            Batches[batch] = values;
        }

        Customers = new Customer[500];
        for (var i = 0; i < Customers.Length; i++)
        {
            var id = i + 1;
            Customers[i] = new Customer(id, "Customer" + id);
        }

        Orders = new Order[5_000];
        for (var i = 0; i < Orders.Length; i++)
        {
            Orders[i] = new Order(
                i + 1,
                i % Customers.Length + 1,
                Regions[i % Regions.Length],
                i * 1237 % 5000 + 1);
        }

        Orders[4_500] = Orders[4_500] with { Amount = 10_000 };
    }

    public static IEnumerable<int> Opaque()
    {
        for (var i = 0; i < 10_000; i++)
        {
            yield return i * 37 % 1000;
        }
    }
}
