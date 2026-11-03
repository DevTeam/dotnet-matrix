using SpanJson;

namespace Matrix.JsonSerialization;

/// <summary>
/// SpanJson formatter for <see cref="Models.Identifier"/>, written against the library's own
/// <see cref="ICustomJsonFormatter{T}"/> contract and registered by type in
/// <see cref="SpanJsonResolver{TSymbol}"/>. It writes the identifier as one JSON string and reads
/// it back into the strongly typed value, which is the library's intended way to teach a scalar
/// value type a JSON representation.
/// </summary>
internal sealed class SpanJsonIdentifierFormatter : ICustomJsonFormatter<Models.Identifier>
{
    /// <summary>
    /// SpanJson discovers a registered formatter through this exact member name and fails at
    /// resolver initialization when it is missing.
    /// </summary>
    public static readonly SpanJsonIdentifierFormatter Default = new();

    public object Arguments { get; set; } = null!;

    public void Serialize(ref JsonWriter<byte> writer, Models.Identifier value) =>
        writer.WriteUtf8String(value.Value);

    public Models.Identifier Deserialize(ref JsonReader<byte> reader) =>
        new(reader.ReadUtf8String());

    public void Serialize(ref JsonWriter<char> writer, Models.Identifier value) =>
        writer.WriteUtf16String(value.Value);

    public Models.Identifier Deserialize(ref JsonReader<char> reader) =>
        new(reader.ReadUtf16String());
}
