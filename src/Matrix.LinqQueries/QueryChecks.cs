namespace Matrix.LinqQueries;

internal static class QueryChecks
{
    [Conditional("MATRIX_VALIDATION")]
    public static void FilterCount(string library, int actual) =>
        MatrixValidation.Require(library, actual == QueryExpectations.FilterCount, "Filter count differs.");

    [Conditional("MATRIX_VALIDATION")]
    public static void ProjectToArray(string library, int[] actual) =>
        Exact(library, actual, QueryExpectations.ProjectToArray, "Projected array differs.");

    [Conditional("MATRIX_VALIDATION")]
    public static void FilterProjectToList(string library, List<int> actual) =>
        Exact(library, actual, QueryExpectations.FilterProjectToList, "Filtered order list differs.");

    [Conditional("MATRIX_VALIDATION")]
    public static void ChainedPipeline(string library, int[] actual) =>
        Exact(library, actual, QueryExpectations.ChainedPipeline, "Chained pipeline differs.");

    [Conditional("MATRIX_VALIDATION")]
    public static void CanonicalPipeline(string library, int[] actual) =>
        Exact(library, actual, QueryExpectations.CanonicalPipeline, "Canonical source pipeline differs.");

    [Conditional("MATRIX_VALIDATION")]
    public static void PagedSlice(string library, int[] actual) =>
        Exact(library, actual, QueryExpectations.PagedSlice, "Paged slice differs.");

    [Conditional("MATRIX_VALIDATION")]
    public static void AnyMatch(string library, bool actual) =>
        MatrixValidation.Require(library, actual, "Expected the sentinel to match.");

    [Conditional("MATRIX_VALIDATION")]
    public static void FirstMatch(string library, Order actual) =>
        MatrixValidation.Require(
            library,
            actual.Id == 4_501 && actual.Amount == 10_000,
            "First matching order differs.");

    [Conditional("MATRIX_VALIDATION")]
    public static void FlattenSelectMany(string library, int[] actual) =>
        Exact(library, actual, QueryExpectations.FlattenSelectMany, "Flattened sequence differs.");

    [Conditional("MATRIX_VALIDATION")]
    public static void DistinctValues(string library, int[] actual)
    {
        var copy = (int[])actual.Clone();
        Array.Sort(copy);
        Exact(library, copy, QueryExpectations.DistinctValues, "Distinct values differ.");
    }

    [Conditional("MATRIX_VALIDATION")]
    public static void ZipPairs(string library, int[] actual) =>
        Exact(library, actual, QueryExpectations.ZipPairs, "Zipped products differ.");

    [Conditional("MATRIX_VALIDATION")]
    public static void Aggregate(string library, int actual) =>
        MatrixValidation.Require(library, actual == QueryExpectations.Aggregate, "Aggregate differs.");

    [Conditional("MATRIX_VALIDATION")]
    public static void OrderedTopN(string library, int[] actual) =>
        Exact(library, actual, QueryExpectations.OrderedTopN, "Ordered top identifiers differ.");

    [Conditional("MATRIX_VALIDATION")]
    public static void GroupByAggregate(string library, RegionTotal[] actual)
    {
        var copy = (RegionTotal[])actual.Clone();
        Array.Sort(copy, static (left, right) => string.CompareOrdinal(left.Region, right.Region));
        var expected = (RegionTotal[])QueryExpectations.GroupByAggregate.Clone();
        Array.Sort(expected, static (left, right) => string.CompareOrdinal(left.Region, right.Region));
        Exact(library, copy, expected, "Region totals differ.");
    }

    [Conditional("MATRIX_VALIDATION")]
    public static void JoinLookup(string library, CustomerOrder[] actual)
    {
        var copy = (CustomerOrder[])actual.Clone();
        Array.Sort(copy, static (left, right) => left.OrderId.CompareTo(right.OrderId));
        Exact(library, copy, QueryExpectations.JoinLookup, "Joined customer orders differ.");
    }

    [Conditional("MATRIX_VALIDATION")]
    public static void ValueDelegateFilter(string library, int actual) =>
        MatrixValidation.Require(
            library,
            actual == QueryExpectations.FilterCount,
            "Struct predicate result differs from the lambda filter.");

    private static void Exact<T>(string library, IReadOnlyList<T> actual, IReadOnlyList<T> expected, string message)
    {
        var equal = actual.Count == expected.Count;
        for (var i = 0; equal && i < actual.Count; i++)
        {
            equal = EqualityComparer<T>.Default.Equals(actual[i], expected[i]);
        }

        MatrixValidation.Require(library, equal, message);
    }
}
