namespace Matrix.ObjectMapping.Models;

public sealed class CustomerSource
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public AddressSource Address { get; set; } = new();
}
