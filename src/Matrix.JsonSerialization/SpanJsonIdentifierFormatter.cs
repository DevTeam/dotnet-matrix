using SpanJson;

namespace Matrix.JsonSerialization;

/// <summary>
/// SpanJson formatter for <see cref="Identifier"/>, written against the library's own
/// <see cref="ICustomJsonFormatter{T}"/> contract and registered by type in
/// <see cref="SpanJsonResolver{TSymbol}"/>. It writes the identifier as one JSON string and reads
/// it back into the strongly typed value, which is the library's intended way to teach a scalar
/// value type a JSON representation.
/// </summary>
internal sealed class SpanJsonIdentifierFormatter : ICustomJsonFormatter<Identifier>
{
    /// <summary>
    /// SpanJson discovers a registered formatter through this exact member name and fails at
    /// resolver initialization when it is missing.
    /// </summary>
    /// <summary>SpanJson's default resolver already has an identical public formatter.</summary>
    public static readonly SpanJsonIdentifierFormatter Default = new();

    public object Arguments { get; set; } = null!;

    public void Serialize(ref JsonWriter<byte> writer, Identifier value) =>
        writer.WriteUtf8String(value.Value);

    public Identifier Deserialize(ref JsonReader<byte> reader) =>
        new(reader.ReadUtf8String());

    public void Serialize(ref JsonWriter<char> writer, Identifier value) =>
        writer.WriteUtf16String(value.Value);

    public Identifier Deserialize(ref JsonReader<char> reader) =>
        new(reader.ReadUtf16String());
}
