namespace Matrix.ObjectMapping.Models;

public sealed class ConversionDestination
{
    public MappingCode Code { get; set; } = new(string.Empty);

    public decimal Amount { get; set; }
}
