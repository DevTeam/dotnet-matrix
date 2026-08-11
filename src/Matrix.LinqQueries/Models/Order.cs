namespace Matrix.LinqQueries.Models;

public sealed record Order(int Id, int CustomerId, string Region, int Amount);
