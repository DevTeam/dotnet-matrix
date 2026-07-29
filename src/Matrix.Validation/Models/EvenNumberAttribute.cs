namespace Matrix.Validation.Models;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class EvenNumberAttribute : ValidationAttribute
{
    public override bool IsValid(object? value) =>
        value is int number && number % 2 == 0;
}
