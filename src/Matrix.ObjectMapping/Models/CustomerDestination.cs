namespace Matrix.ObjectMapping.Models;

public sealed class CustomerDestination
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public AddressDestination Address { get; set; } = new();
}
