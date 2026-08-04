namespace Matrix.ObjectMapping.Models;

public sealed class OrderSource
{
    public int Id { get; init; }

    public decimal Total { get; init; }

    public CustomerSource Customer { get; init; } = new();
}
