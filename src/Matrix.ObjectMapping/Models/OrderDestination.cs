namespace Matrix.ObjectMapping.Models;

public sealed class OrderDestination
{
    public int Id { get; set; }

    public decimal Total { get; set; }

    public CustomerDestination Customer { get; set; } = new();
}
