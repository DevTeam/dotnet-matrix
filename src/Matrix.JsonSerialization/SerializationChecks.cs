namespace Matrix.JsonSerialization;

internal static class SerializationChecks
{
    [Conditional("MATRIX_VALIDATION")]
    public static void Json(string library, string actual, string expected) =>
        MatrixValidation.Require(
            library,
            string.Equals(actual, expected, StringComparison.Ordinal),
            $"Expected JSON '{expected}', but found '{actual}'.");

    [Conditional("MATRIX_VALIDATION")]
    public static void Bytes(string library, byte[] actual, string expected) =>
        Json(library, System.Text.Encoding.UTF8.GetString(actual), expected);

    [Conditional("MATRIX_VALIDATION")]
    public static void Simple(string library, SimpleModel? actual)
    {
        MatrixValidation.Require(library, actual is not null, "Simple result is null.");
        MatrixValidation.Require(library, actual!.Id == 42, "Simple Id differs.");
        MatrixValidation.Require(library, actual.Name == "Ada", "Simple Name differs.");
        MatrixValidation.Require(library, actual.Active, "Simple Active differs.");
    }

    [Conditional("MATRIX_VALIDATION")]
    public static void Nested(string library, OrderModel? actual)
    {
        MatrixValidation.Require(library, actual is not null, "Nested result is null.");
        MatrixValidation.Require(library, actual!.Id == 73, "Nested order Id differs.");
        MatrixValidation.Require(library, actual.Customer.Name == "Ada", "Customer Name differs.");
        MatrixValidation.Require(
            library,
            actual.Customer.Address.City == "London",
            "Address City differs.");
        MatrixValidation.Require(
            library,
            actual.Customer.Address.PostalCode == "SW1",
            "Address PostalCode differs.");
    }

    [Conditional("MATRIX_VALIDATION")]
    public static void Collection(string library, SimpleModel[]? actual)
    {
        MatrixValidation.Require(library, actual is not null, "Collection result is null.");
        MatrixValidation.Require(library, actual!.Length == 3, "Collection length differs.");
        var expected = SerializationData.Collection();
        for (var index = 0; index < expected.Length; index++)
        {
            MatrixValidation.Require(
                library,
                actual[index].Id == expected[index].Id
                && actual[index].Name == expected[index].Name
                && actual[index].Active == expected[index].Active,
                $"Collection element {index} differs.");
        }
    }

    [Conditional("MATRIX_VALIDATION")]
    public static void Dictionary(string library, Dictionary<string, int>? actual)
    {
        MatrixValidation.Require(library, actual is not null, "Dictionary result is null.");
        MatrixValidation.Require(library, actual!.Count == 3, "Dictionary count differs.");
        MatrixValidation.Require(library, actual["alpha"] == 1, "Dictionary alpha differs.");
        MatrixValidation.Require(library, actual["beta"] == 2, "Dictionary beta differs.");
        MatrixValidation.Require(library, actual["gamma"] == 3, "Dictionary gamma differs.");
    }

    [Conditional("MATRIX_VALIDATION")]
    public static void Enum(string library, EnumModel? actual) =>
        MatrixValidation.Require(
            library,
            actual?.Status == ProcessingStatus.Ready,
            "Enum Status differs.");

    [Conditional("MATRIX_VALIDATION")]
    public static void Identifier(string library, IdentifierModel? actual) =>
        MatrixValidation.Require(
            library,
            actual is not null && actual.Id.Equals(new Identifier("order-42")),
            "Identifier differs.");

    [Conditional("MATRIX_VALIDATION")]
    public static void Zoo(string library, ZooModel? actual)
    {
        MatrixValidation.Require(library, actual is not null, "Zoo result is null.");
        MatrixValidation.Require(library, actual!.Animals.Length == 2, "Animal count differs.");
        MatrixValidation.Require(
            library,
            actual.Animals[0] is CatModel { Name: "Pixel", Lives: 9 },
            "Cat result differs.");
        MatrixValidation.Require(
            library,
            actual.Animals[1] is DogModel { Name: "Rex", GoodBoy: true },
            "Dog result differs.");
    }
}
