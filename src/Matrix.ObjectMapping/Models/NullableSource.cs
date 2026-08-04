namespace Matrix.ObjectMapping.Models;

public sealed class NullableSource
{
    public string? Text { get; init; }

    public AddressSource? Address { get; init; }

    public SimpleSource[]? Items { get; init; }
}
