namespace Matrix.ObjectMapping.Models;

public sealed class OrderSummaryDestination
{
    public int Id { get; set; }

    public decimal Total { get; set; }

    public string CustomerName { get; set; } = string.Empty;

    public string CustomerCity { get; set; } = string.Empty;
}
