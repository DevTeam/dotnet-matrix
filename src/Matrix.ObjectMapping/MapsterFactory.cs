using Mapster;
using System.Globalization;

namespace Matrix.ObjectMapping;

internal static class MapsterFactory
{
    public static TypeAdapterConfig CreateConfiguration()
    {
        var configuration = new TypeAdapterConfig();
        configuration.NewConfig<SimpleSource, SimpleDestination>();
        configuration.NewConfig<SimpleSource[], SimpleDestination[]>();
        configuration.NewConfig<AddressSource, AddressDestination>();
        configuration.NewConfig<CustomerSource, CustomerDestination>();
        configuration.NewConfig<OrderSource, OrderDestination>();
        configuration.NewConfig<OrderSource, OrderSummaryDestination>()
            .Map(destination => destination.CustomerName, source => source.Customer.Name)
            .Map(destination => destination.CustomerCity, source => source.Customer.Address.City);
        configuration.NewConfig<NullableSource, NullableDestination>();
        configuration.NewConfig<ConversionSource, ConversionDestination>()
            .Map(destination => destination.Code, source => new MappingCode(source.Code))
            .Map(
                destination => destination.Amount,
                source => decimal.Parse(source.Amount, CultureInfo.InvariantCulture));
        configuration.NewConfig<AnimalSource, AnimalDestination>()
            .Include<CatSource, CatDestination>()
            .Include<DogSource, DogDestination>();
        configuration.NewConfig<CatSource, CatDestination>();
        configuration.NewConfig<DogSource, DogDestination>();
        configuration.NewConfig<AnimalSource[], AnimalDestination[]>();
        configuration.Compile();
        return configuration;
    }
}
