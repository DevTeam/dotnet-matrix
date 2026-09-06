namespace Matrix;

public interface IMatrixReportStore : IMatrixReportReader
{
    void Write<T>(string fileName, T value);

    void WarnEnvironmentMismatch(
        IReadOnlyCollection<BenchmarkEnvironment> existing,
        BenchmarkEnvironment current);
}