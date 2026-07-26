namespace Matrix.DependencyInjection.Scenarios;

public interface ICalculator
{
    int Add(int left, int right);
}

public sealed class Calculator : ICalculator
{
    public int Add(int left, int right) => left + right;
}
