using System.Diagnostics.CodeAnalysis;

namespace Matrix.JsonSerialization.Models;

[SuppressMessage("Usage", "CA2231:Overload operator equals on overriding value type Equals")]
public readonly struct Identifier(string value) : IEquatable<Identifier>
{
    public string Value { get; } = value;

    public bool Equals(Identifier other) =>
        string.Equals(Value, other.Value, StringComparison.Ordinal);

    public override bool Equals(object? obj) =>
        obj is Identifier other && Equals(other);

    public override int GetHashCode() =>
        StringComparer.Ordinal.GetHashCode(Value);

    public override string ToString() => Value;
}
