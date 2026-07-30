namespace Matrix.CsvProcessing.Models;

public readonly record struct CsvAggregate(
    int Count,
    long IdSum,
    decimal AmountSum,
    int ActiveCount);

