using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Matrix.JsonSerialization;

internal static class JsonConfiguration
{
    public static JsonSerializerOptions SystemTextDefault { get; } = new();

    public static JsonSerializerOptions SystemTextEnum { get; } = CreateSystemTextEnum();

    public static JsonSerializerOptions SystemTextIdentifier { get; } =
        CreateSystemTextIdentifier();

    public static JsonSerializerSettings NewtonsoftDefault { get; } = new();

    public static JsonSerializerSettings NewtonsoftEnum { get; } = CreateNewtonsoftEnum();

    public static JsonSerializerSettings NewtonsoftIdentifier { get; } =
        CreateNewtonsoftIdentifier();

    public static JsonSerializerSettings NewtonsoftPolymorphic { get; } =
        CreateNewtonsoftPolymorphic();

    public static JsonSerializerOptions CreateSystemTextDefault() => new();

    public static JsonSerializerSettings CreateNewtonsoftDefault() =>
        new()
        {
            ContractResolver = new DefaultContractResolver()
        };

    private static JsonSerializerOptions CreateSystemTextEnum()
    {
        var options = new JsonSerializerOptions();
        options.Converters.Add(new JsonStringEnumConverter<ProcessingStatus>());
        return options;
    }

    private static JsonSerializerOptions CreateSystemTextIdentifier()
    {
        var options = new JsonSerializerOptions();
        options.Converters.Add(new SystemTextIdentifierConverter());
        return options;
    }

    private static JsonSerializerSettings CreateNewtonsoftEnum() =>
        new()
        {
            Converters = { new StringEnumConverter() }
        };

    private static JsonSerializerSettings CreateNewtonsoftIdentifier() =>
        new()
        {
            Converters = { new NewtonsoftIdentifierConverter() }
        };

    private static JsonSerializerSettings CreateNewtonsoftPolymorphic() =>
        new()
        {
            Converters = { new NewtonsoftAnimalConverter() }
        };
}
