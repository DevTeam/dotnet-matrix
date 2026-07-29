using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using System.Globalization;

namespace Matrix.ObjectMapping;

internal static class AutoMapperFactory
{
    public static MapperConfiguration CreateConfiguration()
    {
        var configuration = new MapperConfiguration(
            expression =>
            {
                expression.AllowNullCollections = true;
                expression.AllowNullDestinationValues = true;
                expression.CreateMap<SimpleSource, SimpleDestination>();
                expression.CreateMap<AddressSource, AddressDestination>();
                expression.CreateMap<CustomerSource, CustomerDestination>();
                expression.CreateMap<OrderSource, OrderDestination>();
                expression.CreateMap<OrderSource, OrderSummaryDestination>()
                    .ForMember(
                        destination => destination.CustomerName,
                        options => options.MapFrom(source => source.Customer.Name))
                    .ForMember(
                        destination => destination.CustomerCity,
                        options => options.MapFrom(source => source.Customer.Address.City));
                expression.CreateMap<NullableSource, NullableDestination>();
                expression.CreateMap<string, MappingCode>()
                    .ConvertUsing(value => new MappingCode(value));
                expression.CreateMap<string, decimal>()
                    .ConvertUsing(value => decimal.Parse(value, CultureInfo.InvariantCulture));
                expression.CreateMap<ConversionSource, ConversionDestination>();
                expression.CreateMap<AnimalSource, AnimalDestination>()
                    .Include<CatSource, CatDestination>()
                    .Include<DogSource, DogDestination>();
                expression.CreateMap<CatSource, CatDestination>();
                expression.CreateMap<DogSource, DogDestination>();
            },
            NullLoggerFactory.Instance);
        configuration.CompileMappings();
        return configuration;
    }

    public static IMapper CreateMapper() => CreateConfiguration().CreateMapper();
}
