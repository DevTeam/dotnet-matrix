namespace Matrix.Validation;

internal static class ValidationData
{
    public static BasicInput Valid() =>
        new()
        {
            Name = "Ada",
            Email = "ada@example.test",
            Age = 36
        };

    public static BasicInput SingleFailure() =>
        new()
        {
            Name = string.Empty,
            Email = "ada@example.test",
            Age = 36
        };

    public static BasicInput MultipleFailures() =>
        new()
        {
            Name = string.Empty,
            Email = "not-an-email",
            Age = 12
        };

    public static NestedInput Nested() =>
        new()
        {
            Address = new AddressInput
            {
                Street = "12 Analytical Engine Way",
                PostalCode = string.Empty
            }
        };

    public static CollectionInput Collection() =>
        new()
        {
            Items =
            [
                new LineItemInput { Sku = "A-1", Quantity = 2 },
                new LineItemInput { Sku = "B-2", Quantity = 0 },
                new LineItemInput { Sku = "C-3", Quantity = 4 }
            ]
        };

    public static ConditionalInput Conditional() =>
        new()
        {
            IsBusiness = true,
            TaxId = null
        };

    public static CustomInput Custom() =>
        new()
        {
            Code = 41
        };

    public static AsyncInput Async() =>
        new()
        {
            UserName = "taken"
        };
}
