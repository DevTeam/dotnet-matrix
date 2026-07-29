// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Complex
{
    private static readonly IFirstService HandFirst = new FirstService();

    private static readonly ISecondService HandSecond = new SecondService();

    private static readonly IThirdService HandThird = new ThirdService();

    [Benchmark(Baseline = true)]
    [LibraryBenchmark(LibraryCatalog.HandCoded)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public BenchmarkRoots<ComplexRoot1, ComplexRoot2, ComplexRoot3> HandCoded()
    {
        return new(
            new ComplexRoot1(CreateSubObject3()),
            new ComplexRoot2(CreateSubObject3()),
            new ComplexRoot3(CreateSubObject3()));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static SubObject3 CreateSubObject3() =>
        new(
            new SubObject1(HandFirst),
            new SubObject2(HandFirst, HandSecond),
            HandThird);
}
