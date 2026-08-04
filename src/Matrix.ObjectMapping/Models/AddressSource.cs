namespace Matrix.ObjectMapping.Models;

public sealed class AddressSource
{
    public string Street { get; init; } = string.Empty;

    public string City { get; init; } = string.Empty;

    public string PostalCode { get; init; } = string.Empty;
}
