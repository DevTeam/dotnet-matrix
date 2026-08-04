namespace Matrix.JsonSerialization.Models;

public sealed class CustomerModel
{
    public string Name { get; init; } = string.Empty;

    public AddressModel Address { get; init; } = new();
}
