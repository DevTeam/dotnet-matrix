namespace Matrix.JsonSerialization.Models;

public readonly struct Identifier(string value) : IEquatable<Identifier>
{
    public string Value { get; } = value;

    public static Identifier Parse(string value) => new(value);

    public static Identifier ParseJson(string value) => new(value);

    public bool Equals(Identifier other) =>
        string.Equals(Value, other.Value, StringComparison.Ordinal);

    public override bool Equals(object? obj) =>
        obj is Identifier other && Equals(other);

    public override int GetHashCode() =>
        StringComparer.Ordinal.GetHashCode(Value);

    public override string ToString() => Value;
}
