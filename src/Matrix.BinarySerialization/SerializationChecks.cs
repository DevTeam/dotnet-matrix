namespace Matrix.BinarySerialization;



internal static class SerializationChecks

{

    [Conditional("MATRIX_VALIDATION")]

    public static void NonEmpty(string library, byte[] payload) =>

        MatrixValidation.Require(

            library,

            payload.Length > 0,

            "Serialized payload is empty.");



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

    public static void Collection(string library, CollectionModel? actual)
    {
        MatrixValidation.Require(library, actual is not null, "Collection model is null.");
        var items = actual!.Items;
        MatrixValidation.Require(library, items is not null, "Collection items are null.");
        MatrixValidation.Require(library, items!.Count == 3, "Collection length differs.");
        MatrixValidation.Require(library, items[0].Id == 1 && items[0].Name == "One", "Element 0 differs.");
        MatrixValidation.Require(library, items[1].Id == 2 && !items[1].Active, "Element 1 differs.");
        MatrixValidation.Require(library, items[2].Id == 3 && items[2].Name == "Three", "Element 2 differs.");
    }

    [Conditional("MATRIX_VALIDATION")]

    public static void Enum(string library, EnumModel? actual) =>

        MatrixValidation.Require(

            library,

            actual?.Status == ProcessingStatus.Ready,

            "Enum Status differs.");



    [Conditional("MATRIX_VALIDATION")]

    public static void SimplePayload(string library, byte[] payload) =>

        MatrixValidation.Require(library, payload.Length > 0, "Serialized payload is empty.");



    [Conditional("MATRIX_VALIDATION")]

    public static void SimpleMessage(string library, SimpleMessage? actual)

    {

        MatrixValidation.Require(library, actual is not null, "Simple message is null.");

        MatrixValidation.Require(library, actual!.Id == 42, "Simple message Id differs.");

        MatrixValidation.Require(library, actual.Name == "Ada", "Simple message Name differs.");

        MatrixValidation.Require(library, actual.Active, "Simple message Active differs.");

    }



    [Conditional("MATRIX_VALIDATION")]

    public static void NestedMessage(string library, OrderMessage? actual)

    {

        MatrixValidation.Require(library, actual is not null, "Order message is null.");

        MatrixValidation.Require(library, actual!.Id == 73, "Order message Id differs.");

        MatrixValidation.Require(

            library,

            actual.Customer is not null && actual.Customer.Name == "Ada",

            "Order message customer name differs.");

        MatrixValidation.Require(

            library,

            actual.Customer!.Address is not null

            && actual.Customer.Address.City == "London"

            && actual.Customer.Address.PostalCode == "SW1",

            "Order message address differs.");

    }



    [Conditional("MATRIX_VALIDATION")]

    public static void MessageCollection(string library, IReadOnlyList<SimpleMessage>? actual)

    {

        MatrixValidation.Require(library, actual is not null, "Message collection is null.");

        MatrixValidation.Require(library, actual!.Count == 3, "Message collection length differs.");

        MatrixValidation.Require(library, actual[0].Id == 1 && actual[0].Name == "One", "Element 0 differs.");

        MatrixValidation.Require(library, actual[1].Id == 2 && !actual[1].Active, "Element 1 differs.");

        MatrixValidation.Require(library, actual[2].Id == 3 && actual[2].Name == "Three", "Element 2 differs.");

    }



    [Conditional("MATRIX_VALIDATION")]

    public static void EnumMessage(string library, EnumMessage? actual)

    {

        MatrixValidation.Require(library, actual is not null, "Enum message is null.");

        MatrixValidation.Require(

            library,

            actual!.Status == ProcessingStatusMessage.Ready,

            "Enum message status differs.");

    }



    [Conditional("MATRIX_VALIDATION")]

    public static void SimpleTable(string library, SimpleTable? actual)

    {

        MatrixValidation.Require(library, actual is not null, "Simple table is null.");

        MatrixValidation.Require(library, actual!.Id == 42, "Simple table Id differs.");

        MatrixValidation.Require(library, actual.Name == "Ada", "Simple table Name differs.");

        MatrixValidation.Require(library, actual.Active, "Simple table Active differs.");

    }



    [Conditional("MATRIX_VALIDATION")]

    public static void NestedTable(string library, OrderTable? actual)

    {

        MatrixValidation.Require(library, actual is not null, "Order table is null.");

        MatrixValidation.Require(library, actual!.Id == 73, "Order table Id differs.");

        MatrixValidation.Require(

            library,

            actual.Customer is not null && actual.Customer.Name == "Ada",

            "Order table customer name differs.");

        MatrixValidation.Require(

            library,

            actual.Customer!.Address is not null

            && actual.Customer.Address.City == "London"

            && actual.Customer.Address.PostalCode == "SW1",

            "Order table address differs.");

    }



    [Conditional("MATRIX_VALIDATION")]

    public static void TableCollection(string library, IList<SimpleTable>? actual)

    {

        MatrixValidation.Require(library, actual is not null, "Table collection is null.");

        MatrixValidation.Require(library, actual!.Count == 3, "Table collection length differs.");

        MatrixValidation.Require(library, actual[0].Id == 1 && actual[0].Name == "One", "Element 0 differs.");

        MatrixValidation.Require(library, actual[1].Id == 2 && !actual[1].Active, "Element 1 differs.");

        MatrixValidation.Require(library, actual[2].Id == 3 && actual[2].Name == "Three", "Element 2 differs.");

    }



    [Conditional("MATRIX_VALIDATION")]

    public static void EnumTable(string library, EnumTable? actual)

    {

        MatrixValidation.Require(library, actual is not null, "Enum table is null.");

        MatrixValidation.Require(

            library,

            actual!.Status == (int)ProcessingStatus.Ready,

            "Enum table status differs.");

    }

}

