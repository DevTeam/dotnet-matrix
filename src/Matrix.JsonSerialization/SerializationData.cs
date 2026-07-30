namespace Matrix.JsonSerialization;

public static class SerializationData
{
    public const string SimpleJson = """{"Id":42,"Name":"Ada","Active":true}""";

    public const string NestedJson =
        """{"Id":73,"Customer":{"Name":"Ada","Address":{"City":"London","PostalCode":"SW1"}}}""";

    public const string CollectionJson =
        """[{"Id":1,"Name":"One","Active":true},{"Id":2,"Name":"Two","Active":false},{"Id":3,"Name":"Three","Active":true}]""";

    public const string DictionaryJson = """{"alpha":1,"beta":2,"gamma":3}""";

    public const string EnumJson = """{"Status":"Ready"}""";

    public const string IdentifierJson = """{"Id":"order-42"}""";

    public const string PolymorphicJson =
        """{"Animals":[{"$type":"cat","Lives":9,"Name":"Pixel"},{"$type":"dog","GoodBoy":true,"Name":"Rex"}]}""";

    public static SimpleModel Simple() =>
        new()
        {
            Id = 42,
            Name = "Ada",
            Active = true
        };

    public static OrderModel Nested() =>
        new()
        {
            Id = 73,
            Customer = new CustomerModel
            {
                Name = "Ada",
                Address = new AddressModel
                {
                    City = "London",
                    PostalCode = "SW1"
                }
            }
        };

    public static SimpleModel[] Collection() =>
    [
        new SimpleModel { Id = 1, Name = "One", Active = true },
        new SimpleModel { Id = 2, Name = "Two", Active = false },
        new SimpleModel { Id = 3, Name = "Three", Active = true }
    ];

    public static Dictionary<string, int> Dictionary() =>
        new(StringComparer.Ordinal)
        {
            ["alpha"] = 1,
            ["beta"] = 2,
            ["gamma"] = 3
        };

    public static EnumModel Enum() =>
        new()
        {
            Status = ProcessingStatus.Ready
        };

    public static IdentifierModel Identifier() =>
        new()
        {
            Id = new Identifier("order-42")
        };

    public static ZooModel Zoo() =>
        new()
        {
            Animals =
            [
                new CatModel { Name = "Pixel", Lives = 9 },
                new DogModel { Name = "Rex", GoodBoy = true }
            ]
        };
}
