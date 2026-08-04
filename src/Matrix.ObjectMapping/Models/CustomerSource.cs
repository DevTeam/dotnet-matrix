namespace Matrix.ObjectMapping.Models;

public sealed class CustomerSource
{
    public int Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public AddressSource Address { get; init; } = new();
}
