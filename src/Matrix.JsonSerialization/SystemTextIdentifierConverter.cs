using System.Text.Json;
using System.Text.Json.Serialization;

namespace Matrix.JsonSerialization;

internal sealed class SystemTextIdentifierConverter : JsonConverter<Identifier>
{
    public override Identifier Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options) =>
        new(reader.GetString() ?? string.Empty);

    public override void Write(
        Utf8JsonWriter writer,
        Identifier value,
        JsonSerializerOptions options) =>
        writer.WriteStringValue(value.Value);
}
