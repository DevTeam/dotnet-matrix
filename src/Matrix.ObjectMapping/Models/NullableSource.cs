namespace Matrix.ObjectMapping.Models;

public sealed class NullableSource
{
    public string? Text { get; set; }

    public AddressSource? Address { get; set; }

    public SimpleSource[]? Items { get; set; }
}
