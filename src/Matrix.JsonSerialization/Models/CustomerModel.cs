namespace Matrix.JsonSerialization.Models;

public sealed class CustomerModel
{
    public string Name { get; set; } = string.Empty;

    public AddressModel Address { get; set; } = new();
}
