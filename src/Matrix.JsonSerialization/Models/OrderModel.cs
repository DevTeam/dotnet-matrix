namespace Matrix.JsonSerialization.Models;

public sealed class OrderModel
{
    public int Id { get; init; }

    public CustomerModel Customer { get; init; } = new();
}
