namespace Matrix.ObjectMapping;

internal static class ObjectMappingValidation
{
    public static void Simple(
        string library,
        SimpleSource source,
        SimpleDestination destination,
        SimpleDestination? previous = null)
    {
        MatrixValidation.Require(library, destination.Id == source.Id, "Simple Id differs.");
        MatrixValidation.Require(library, destination.Name == source.Name, "Simple Name differs.");
        MatrixValidation.Require(library, destination.Amount == source.Amount, "Simple Amount differs.");
        MatrixValidation.Require(
            library,
            destination.CreatedAt == source.CreatedAt,
            "Simple CreatedAt differs.");
        MatrixValidation.Require(library, destination.Active == source.Active, "Simple Active differs.");
        if (previous is not null)
        {
            MatrixValidation.Different(
                library,
                destination,
                previous,
                "Simple mapping reused the destination instance.");
        }
    }

    public static void Nested(
        string library,
        OrderSource source,
        OrderDestination destination,
        OrderDestination? previous = null)
    {
        MatrixValidation.Require(library, destination.Id == source.Id, "Nested order Id differs.");
        MatrixValidation.Require(
            library,
            destination.Total == source.Total,
            "Nested order Total differs.");
        MatrixValidation.Require(
            library,
            destination.Customer.Id == source.Customer.Id,
            "Nested customer Id differs.");
        MatrixValidation.Require(
            library,
            destination.Customer.Name == source.Customer.Name,
            "Nested customer Name differs.");
        MatrixValidation.Require(
            library,
            destination.Customer.Address.Street == source.Customer.Address.Street,
            "Nested address Street differs.");
        MatrixValidation.Require(
            library,
            destination.Customer.Address.City == source.Customer.Address.City,
            "Nested address City differs.");
        MatrixValidation.Require(
            library,
            destination.Customer.Address.PostalCode == source.Customer.Address.PostalCode,
            "Nested address PostalCode differs.");
        if (previous is not null)
        {
            MatrixValidation.Different(
                library,
                destination,
                previous,
                "Nested mapping reused the order destination.");
            MatrixValidation.Different(
                library,
                destination.Customer,
                previous.Customer,
                "Nested mapping reused the customer destination.");
            MatrixValidation.Different(
                library,
                destination.Customer.Address,
                previous.Customer.Address,
                "Nested mapping reused the address destination.");
        }
    }

    public static void Collection(
        string library,
        SimpleSource[] source,
        SimpleDestination[] destination,
        SimpleDestination[]? previous = null)
    {
        MatrixValidation.Require(
            library,
            destination.Length == source.Length,
            "Collection length differs.");
        for (var index = 0; index < source.Length; index++)
        {
            Simple(library, source[index], destination[index]);
            if (previous is not null)
            {
                MatrixValidation.Different(
                    library,
                    destination[index],
                    previous[index],
                    $"Collection element {index} was reused.");
            }
        }

        if (previous is not null)
        {
            MatrixValidation.Different(
                library,
                destination,
                previous,
                "Collection mapping reused the destination array.");
        }
    }

    public static void Flattening(
        string library,
        OrderSource source,
        OrderSummaryDestination destination,
        OrderSummaryDestination? previous = null)
    {
        MatrixValidation.Require(library, destination.Id == source.Id, "Flattened Id differs.");
        MatrixValidation.Require(
            library,
            destination.Total == source.Total,
            "Flattened Total differs.");
        MatrixValidation.Require(
            library,
            destination.CustomerName == source.Customer.Name,
            "Flattened CustomerName differs.");
        MatrixValidation.Require(
            library,
            destination.CustomerCity == source.Customer.Address.City,
            "Flattened CustomerCity differs.");
        if (previous is not null)
        {
            MatrixValidation.Different(
                library,
                destination,
                previous,
                "Flattening reused the destination instance.");
        }
    }

    public static void Existing(
        string library,
        SimpleSource source,
        SimpleDestination expected,
        SimpleDestination actual)
    {
        MatrixValidation.Same(
            library,
            expected,
            actual,
            "Map To Existing replaced the supplied destination.");
        Simple(library, source, actual);
    }

    public static void NullHandling(
        string library,
        NullableDestination destination,
        NullableDestination? previous = null)
    {
        MatrixValidation.Require(library, destination.Text is null, "Null text was not preserved.");
        MatrixValidation.Require(
            library,
            destination.Address is null,
            "Null nested object was not preserved.");
        MatrixValidation.Require(
            library,
            destination.Items is null,
            "Null collection was not preserved.");
        if (previous is not null)
        {
            MatrixValidation.Different(
                library,
                destination,
                previous,
                "Null mapping reused the destination instance.");
        }
    }

    public static void Conversion(
        string library,
        ConversionDestination destination,
        ConversionDestination? previous = null)
    {
        MatrixValidation.Require(
            library,
            destination.Code.Value == "MATRIX-42",
            "Custom code conversion differs.");
        MatrixValidation.Require(
            library,
            destination.Amount == 1234.50m,
            "Custom decimal conversion differs.");
        if (previous is not null)
        {
            MatrixValidation.Different(
                library,
                destination,
                previous,
                "Custom conversion reused the destination instance.");
        }
    }

    public static void Polymorphic(
        string library,
        AnimalSource[] source,
        AnimalDestination[] destination,
        AnimalDestination[]? previous = null)
    {
        MatrixValidation.Require(
            library,
            destination.Length == source.Length,
            "Polymorphic collection length differs.");
        MatrixValidation.Require(
            library,
            destination[0] is CatDestination { Name: "Pixel", Lives: 9 },
            "First polymorphic result is invalid.");
        MatrixValidation.Require(
            library,
            destination[1] is DogDestination { Name: "Rex", GoodBoy: true },
            "Second polymorphic result is invalid.");
        MatrixValidation.Require(
            library,
            destination[2] is CatDestination { Name: "Mochi", Lives: 7 },
            "Third polymorphic result is invalid.");
        if (previous is not null)
        {
            MatrixValidation.Different(
                library,
                destination,
                previous,
                "Polymorphic mapping reused the destination array.");
            for (var index = 0; index < destination.Length; index++)
            {
                MatrixValidation.Different(
                    library,
                    destination[index],
                    previous[index],
                    $"Polymorphic element {index} was reused.");
            }
        }
    }
}
