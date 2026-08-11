using StructLinq;

namespace Matrix.LinqQueries.Models;

public readonly struct DivisibleByThree : INumberPredicate, IFunction<int, bool>
{
    public bool Match(int value) => value % 3 == 0;

    public bool Eval(int element) => Match(element);
}
