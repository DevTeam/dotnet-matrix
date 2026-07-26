// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.DependencyInjection.Benchmarks;

public readonly struct BenchmarkRoots<T1, T2>(T1 first, T2 second)
{
    public T1 First { get; } = first;

    public T2 Second { get; } = second;
}

public readonly struct BenchmarkRoots<T1, T2, T3>(T1 first, T2 second, T3 third)
{
    public T1 First { get; } = first;

    public T2 Second { get; } = second;

    public T3 Third { get; } = third;
}
