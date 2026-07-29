using Riok.Mapperly.Abstractions;
using System.Globalization;

namespace Matrix.ObjectMapping;

[Mapper(AllowNullPropertyAssignment = true)]
internal partial class MapperlyMapper
{
    public partial SimpleDestination MapSimple(SimpleSource source);

    public partial OrderDestination MapOrder(OrderSource source);

    public partial SimpleDestination[] MapCollection(SimpleSource[] source);

    [MapProperty("Customer.Name", "CustomerName")]
    [MapProperty("Customer.Address.City", "CustomerCity")]
    public partial OrderSummaryDestination MapSummary(OrderSource source);

    public partial void MapExisting(SimpleSource source, SimpleDestination destination);

    public partial NullableDestination MapNullable(NullableSource source);

    public partial ConversionDestination MapConversion(ConversionSource source);

    [MapDerivedType<CatSource, CatDestination>]
    [MapDerivedType<DogSource, DogDestination>]
    public partial AnimalDestination MapAnimal(AnimalSource source);

    public partial AnimalDestination[] MapAnimals(AnimalSource[] source);

    private static MappingCode MapCode(string value) => new(value);

    private static decimal MapDecimal(string value) =>
        decimal.Parse(value, CultureInfo.InvariantCulture);
}
