namespace Matrix.ObjectMapping.Benchmarks;

public partial class NestedObject
{
    [Benchmark(Baseline = true)]
    [LibraryBenchmark(LibraryCatalog.HandCoded)]
    public OrderDestination HandCoded()
    {
        var destination = new OrderDestination
        {
            Id = _source.Id,
            Total = _source.Total,
            Customer = new CustomerDestination
            {
                Id = _source.Customer.Id,
                Name = _source.Customer.Name,
                Address = new AddressDestination
                {
                    Street = _source.Customer.Address.Street,
                    City = _source.Customer.Address.City,
                    PostalCode = _source.Customer.Address.PostalCode
                }
            }
        };
        Validate(LibraryCatalog.HandCoded, destination);
        return destination;
    }
}
