namespace Matrix.JsonSerialization.Models;

public sealed class OrderModel
{
    public int Id { get; set; }

    public CustomerModel Customer { get; set; } = new();
}
