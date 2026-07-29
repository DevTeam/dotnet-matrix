namespace Matrix.ObjectMapping.Models;

public sealed class NullableDestination
{
    public string? Text { get; set; }

    public AddressDestination? Address { get; set; }

    public SimpleDestination[]? Items { get; set; }
}
