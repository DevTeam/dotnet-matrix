namespace Matrix.ObjectMapping;

internal static class ObjectMappingData
{
    public static SimpleSource Simple(int id = 42) =>
        new()
        {
            Id = id,
            Name = $"Source {id}",
            Amount = 1234.5m + id,
            CreatedAt = new DateTime(2026, 7, 29, 12, 34, 56, DateTimeKind.Utc)
                .AddMinutes(id),
            Active = id % 2 == 0
        };

    public static OrderSource Order() =>
        new()
        {
            Id = 73,
            Total = 9876.54m,
            Customer = new CustomerSource
            {
                Id = 19,
                Name = "Ada Lovelace",
                Address = new AddressSource
                {
                    Street = "12 Analytical Engine Way",
                    City = "London",
                    PostalCode = "SW1A 1AA"
                }
            }
        };

    public static SimpleSource[] Collection() =>
        Enumerable.Range(1, 100).Select(Simple).ToArray();

    public static NullableSource Nullable() =>
        new()
        {
            Text = null,
            Address = null,
            Items = null
        };

    public static ConversionSource Conversion() =>
        new()
        {
            Code = "MATRIX-42",
            Amount = "1234.50"
        };

    public static AnimalSource[] Animals() =>
    [
        new CatSource { Name = "Pixel", Lives = 9 },
        new DogSource { Name = "Rex", GoodBoy = true },
        new CatSource { Name = "Mochi", Lives = 7 }
    ];
}
