namespace Matrix.ObjectMapping.Models;

public sealed class OrderSource
{
    public int Id { get; set; }

    public decimal Total { get; set; }

    public CustomerSource Customer { get; set; } = new();
}
