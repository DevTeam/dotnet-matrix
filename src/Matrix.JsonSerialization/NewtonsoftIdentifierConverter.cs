using Newtonsoft.Json;

namespace Matrix.JsonSerialization;

internal sealed class NewtonsoftIdentifierConverter : JsonConverter<Identifier>
{
    public override Identifier ReadJson(
        JsonReader reader,
        Type objectType,
        Identifier existingValue,
        bool hasExistingValue,
        JsonSerializer serializer) =>
        new((string?)reader.Value ?? string.Empty);

    public override void WriteJson(
        JsonWriter writer,
        Identifier value,
        JsonSerializer serializer) =>
        writer.WriteValue(value.Value);
}
