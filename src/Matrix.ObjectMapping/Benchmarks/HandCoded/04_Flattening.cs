namespace Matrix.ObjectMapping.Benchmarks;

public partial class Flattening
{
    [Benchmark(Baseline = true)]
    [LibraryBenchmark(LibraryCatalog.HandCoded)]
    public OrderSummaryDestination HandCoded()
    {
        var destination = new OrderSummaryDestination
        {
            Id = _source.Id,
            Total = _source.Total,
            CustomerName = _source.Customer.Name,
            CustomerCity = _source.Customer.Address.City
        };
        Validate(LibraryCatalog.HandCoded, destination);
        return destination;
    }
}
