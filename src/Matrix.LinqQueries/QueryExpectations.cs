namespace Matrix.LinqQueries;

internal static class QueryExpectations
{
    public static readonly int FilterCount;
    public static readonly int[] ProjectToArray;
    public static readonly List<int> FilterProjectToList;
    public static readonly int[] ChainedPipeline;
    public static readonly int[] CanonicalPipeline;
    public static readonly int[] PagedSlice;
    public static readonly int[] FlattenSelectMany;
    public static readonly int[] DistinctValues;
    public static readonly int[] ZipPairs;
    public static readonly int Aggregate;
    public static readonly int[] OrderedTopN;
    public static readonly RegionTotal[] GroupByAggregate;
    public static readonly CustomerOrder[] JoinLookup;

    static QueryExpectations()
    {
        var projected = new int[QueryData.Numbers.Length];
        var filteredOrders = new List<int>();
        var chained = new List<int>(1000);
        var canonical = new List<int>();
        var aggregate = 0;
        var filterCount = 0;
        for (var i = 0; i < QueryData.Numbers.Length; i++)
        {
            var value = QueryData.Numbers[i];
            projected[i] = value * 2;
            aggregate += value;
            if (value % 3 != 0)
            {
                continue;
            }

            filterCount++;
            var projection = value * 2;
            canonical.Add(projection);
            if (projection % 4 == 0 && chained.Count < 1000)
            {
                chained.Add(projection);
            }
        }

        for (var i = 0; i < QueryData.Orders.Length; i++)
        {
            var order = QueryData.Orders[i];
            if (order.Amount > 2500)
            {
                filteredOrders.Add(order.Id);
            }
        }

        FilterCount = filterCount;
        ProjectToArray = projected;
        FilterProjectToList = filteredOrders;
        ChainedPipeline = chained.ToArray();
        CanonicalPipeline = canonical.ToArray();
        Aggregate = aggregate;

        PagedSlice = new int[1000];
        for (var i = 0; i < PagedSlice.Length; i++)
        {
            PagedSlice[i] = QueryData.Numbers[i + 4000];
        }

        FlattenSelectMany = new int[10_000];
        var flattenedIndex = 0;
        for (var batch = 0; batch < QueryData.Batches.Length; batch++)
        for (var index = 0; index < QueryData.Batches[batch].Length; index++)
        {
            FlattenSelectMany[flattenedIndex++] = QueryData.Batches[batch][index];
        }

        var seen = new bool[1000];
        for (var i = 0; i < QueryData.Numbers.Length; i++)
        {
            seen[QueryData.Numbers[i]] = true;
        }

        var distinct = new List<int>(1000);
        for (var value = 0; value < seen.Length; value++)
        {
            if (seen[value])
            {
                distinct.Add(value);
            }
        }

        DistinctValues = distinct.ToArray();

        ZipPairs = new int[QueryData.Numbers.Length];
        for (var i = 0; i < ZipPairs.Length; i++)
        {
            ZipPairs[i] = QueryData.Numbers[i] * QueryData.Numbers[i];
        }

        OrderedTopN = BuildOrderedTopN();
        GroupByAggregate = BuildRegionTotals();
        JoinLookup = BuildJoin();
    }

    private static int[] BuildOrderedTopN()
    {
        const int count = 20;
        var amounts = new int[count];
        var ids = new int[count];
        var filled = 0;
        for (var orderIndex = 0; orderIndex < QueryData.Orders.Length; orderIndex++)
        {
            var order = QueryData.Orders[orderIndex];
            var insertAt = filled;
            while (insertAt > 0 && amounts[insertAt - 1] < order.Amount)
            {
                insertAt--;
            }

            if (insertAt >= count)
            {
                continue;
            }

            var last = Math.Min(filled, count - 1);
            for (var i = last; i > insertAt; i--)
            {
                amounts[i] = amounts[i - 1];
                ids[i] = ids[i - 1];
            }

            amounts[insertAt] = order.Amount;
            ids[insertAt] = order.Id;
            if (filled < count)
            {
                filled++;
            }
        }

        return ids;
    }

    private static RegionTotal[] BuildRegionTotals()
    {
        var totals = new int[QueryData.Regions.Length];
        for (var orderIndex = 0; orderIndex < QueryData.Orders.Length; orderIndex++)
        {
            var order = QueryData.Orders[orderIndex];
            for (var regionIndex = 0; regionIndex < QueryData.Regions.Length; regionIndex++)
            {
                if (string.Equals(order.Region, QueryData.Regions[regionIndex], StringComparison.Ordinal))
                {
                    totals[regionIndex] += order.Amount;
                    break;
                }
            }
        }

        var result = new RegionTotal[totals.Length];
        for (var i = 0; i < totals.Length; i++)
        {
            result[i] = new RegionTotal(QueryData.Regions[i], totals[i]);
        }

        return result;
    }

    private static CustomerOrder[] BuildJoin()
    {
        var result = new CustomerOrder[QueryData.Orders.Length];
        for (var orderIndex = 0; orderIndex < QueryData.Orders.Length; orderIndex++)
        {
            var order = QueryData.Orders[orderIndex];
            for (var customerIndex = 0; customerIndex < QueryData.Customers.Length; customerIndex++)
            {
                var customer = QueryData.Customers[customerIndex];
                if (order.CustomerId == customer.Id)
                {
                    result[orderIndex] = new CustomerOrder(order.Id, customer.Name);
                    break;
                }
            }
        }

        return result;
    }
}
